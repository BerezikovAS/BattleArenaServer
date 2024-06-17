using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.GeomantSkills
{
    public class EarthquakeSkill : Skill
    {
        int dmg = 100;
        int extraDmg = 18;
        public EarthquakeSkill()
        {
            name = "Earthquake";
            title = $"Землятрясение поражает врагов вокруг, нанося магический урон и сбивая с ног, отчего те теряют 1 ОД.\n" +
                $"Урон зависит от количества свободных клеток вокруг цели (X). Столоктиты считаются свободными гексами и дают двойной бонус.\n" +
                $"({dmg} + {extraDmg} * X)";
            titleUpg = "+10 к урону за гекс. +1 к радиусу";
            coolDown = 6;
            coolDownNow = 0;
            requireAP = 3;
            radius = 3;
            nonTarget = false;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new NonTargerAoECastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.CasterHex != null)
            {
                // Пробегаемся по клеткам в радиусе и ищем врагов
                foreach (var hex in UtilityService.GetHexesRadius(requestData.CasterHex, radius))
                {
                    if (hex.HERO != null && hex.HERO.Team != requestData.Caster.Team)
                    {
                        // Нашли врага, начинаем считать свободные клетки и столоктиты вокруг него
                        int freeHexCount = 0;
                        foreach (var hexTarget in UtilityService.GetHexesRadius(hex, 1))
                        {
                            if (hexTarget.IsFree())
                                freeHexCount += 1;
                            else if (hexTarget.HERO == null && hexTarget.OBSTACLE != null && hexTarget.OBSTACLE.Name == "Stalaktite")
                                freeHexCount += 2;
                        }
                        hex.HERO.AP -= 1;
                        AttackService.SetDamage(requestData.Caster, hex.HERO, dmg + extraDmg * freeHexCount, Consts.DamageType.Magic);
                    }
                }
                requestData.Caster.AP -= requireAP;
                coolDownNow = coolDown;
                return true;
            }
            return false;
        }

        public override bool UpgradeSkill()
        {
            if (!upgraded)
            {
                upgraded = true;
                extraDmg += 10;
                radius += 1;
                stats.radius += 1;
                return true;
            }
            return false;
        }
    }
}
