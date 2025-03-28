using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.InvokerSkills
{
    public class ForstBreathSkill : Skill
    {
        public ForstBreathSkill()
        {
            name = "Forst Breath";
            dmg = 90;
            title = $"Морозное дыхание наносит врагам в области {dmg} маг. урона и отнимает 1 ОД.";
            titleUpg = "+25 к урону, +1 к дальности";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            range = 2;
            radius = 2;
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
                foreach (var n in UtilityService.GetHexesWideLine(requestData.CasterHex, requestData.TargetHex, radius))
                {
                    if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                    {
                        AttackService.SetDamage(requestData.Caster, n.HERO, dmg, dmgType);
                        n.HERO.AP -= 1;
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
                dmg += 25;
                radius += 1;
                stats.radius += 1;
                range += 1;
                stats.range += 1;
                title = $"Морозное дыхание наносит врагам в области {dmg} маг. урона и отнимает 1 ОД.";
                return true;
            }
            return false;
        }
    }
}
