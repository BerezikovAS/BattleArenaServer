using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.GolemSkills
{
    public class MagnetSkill : Skill
    {
        public MagnetSkill()
        {
            name = "Magnet";
            dmg = 140;
            title = $"Наносит врагам по линиям от себя {dmg} магического урона и притягивает их к себе.";
            titleUpg = "+25 к урону, -1 к перезарядке";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            radius = 2;
            range = 2;
            nonTarget = false;
            area = Consts.SpellArea.Lines;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new NonTargerAoECastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.CasterHex != null)
            {
                List<Hero> enemies = new List<Hero>();
                //Сначала пытаемся пододвинуть всех, а потом будем наносить урон
                foreach (var n in UtilityService.GetHexesLines(requestData.CasterHex, radius))
                {
                    int pos = 1;
                    Hero? target = null;
                    Hex? targetHex = null;
                    while (pos <= radius)
                    {
                        targetHex = UtilityService.GetOneHexOnDirection(requestData.CasterHex, n, pos);
                        target = targetHex?.HERO?.Team != requestData.Caster.Team ? targetHex?.HERO : null;
                        pos++;
                        
                        if (target != null && !enemies.Contains(target))
                            enemies.Add(target);
                    }

                    if (target != null && targetHex != null)
                    {
                        Hex? hex = UtilityService.GetOneHexOnDirection(targetHex, requestData.CasterHex, 1);
                        if (hex != null && hex.IsFree())
                            AttackService.MoveHero(target, targetHex, hex);

                    }
                }

                //Наносим урон
                foreach (var enemy in enemies)
                    AttackService.SetDamage(requestData.Caster, enemy, dmg, dmgType);

                requestData.Caster.SpendAP(requireAP);
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
                dmg += 25;
                coolDown -= 1;
                stats.coolDown -= 1;
                title = $"Наносит врагам по линиям от себя {dmg} магического урона и притягивает их к себе.";
                return true;
            }
            return false;
        }
    }
}
