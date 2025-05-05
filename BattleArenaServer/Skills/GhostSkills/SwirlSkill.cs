using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.GhostSkills
{
    public class SwirlSkill : Skill
    {
        public SwirlSkill()
        {
            name = "Swirl";
            dmg = 150;
            title = $"Мистические вихри проносятся по кругу и наносят врагам {dmg} магического урона.";
            titleUpg = "+1 к дальности, +30 к урону";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 2;
            radius = 2;
            area = Consts.SpellArea.Circle;
            dmgType = Consts.DamageType.Magic;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new RangeAoECastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.CasterHex != null && requestData.TargetHex != null)
            {
                if (requestData.CasterHex.Distance(requestData.TargetHex) < 1)
                    return false;

                foreach (var n in UtilityService.GetHexesCircle(requestData.CasterHex, requestData.TargetHex, radius))
                {
                    if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                    {
                        AttackService.SetDamage(requestData.Caster, n.HERO, dmg, Consts.DamageType.Magic);
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
                range += 1;
                stats.range += 1;
                radius += 1;
                stats.radius += 1;
                dmg += 30;
                title = $"Мистические вихри проносятся по кругу и наносят врагам {dmg} магического урона.";
                return true;
            }
            return false;
        }
    }
}
