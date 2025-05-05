using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.GolemSkills
{
    public class ShockWaveSkill : Skill
    {
        public ShockWaveSkill()
        {
            name = "Shock Wave";
            dmg = 130;
            title = $"Запускает ударную волну, которая наносит {dmg} маг. урона и отталкивает врагов назад.";
            titleUpg = "+30 к урону";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            range = 3;
            radius = 3;
            nonTarget = false;
            area = Consts.SpellArea.WideLine;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new LineCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null && requestData.CasterHex != null)
            {
                List<Hero> affectredHeroes = new List<Hero>();
                Hex direction = UtilityService.GetDirection(requestData.CasterHex, requestData.TargetHex);

                for (int i = radius; i > 0; i--)
                    foreach (var n in UtilityService.GetHexesWideLine(requestData.CasterHex, requestData.TargetHex, radius))
                    {
                        //Сначала отталкиваем самых дальних
                        if (n.HERO != null && n.HERO.Team != requestData.Caster.Team && n.Distance(requestData.CasterHex) == i && !affectredHeroes.Contains(n.HERO))
                        {
                            affectredHeroes.Add(n.HERO);
                            AttackService.SetDamage(requestData.Caster, n.HERO, dmg, dmgType);

                            //Гекс на который нужно оттолкнуть врага
                            Hex? targetHex = UtilityService.GetOneHexOnDirection(n, direction, 1, 1);

                            if (targetHex != null && targetHex.IsFree() && n.HERO != null && n.HERO.HP > 0)
                                AttackService.MoveHero(n.HERO, n, targetHex);
                        }
                    }
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
                dmg += 30;
                title = $"Запускает ударную волну, которая наносит {dmg} маг. урона и отталкивает врагов назад.";
                return true;
            }
            return false;
        }
    }
}
