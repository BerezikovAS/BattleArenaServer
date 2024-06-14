using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.Knight
{
    public class HailStrikeSkill : BodyGuardSkill
    {
        public HailStrikeSkill()
        {
            name = "Hail Strike";
            title = "Обрушивает мощный град на врагов в области, нанося 200 маг. урона каждому";
            titleUpg = "+1 к радиусу, +1 к дальности";
            coolDown = 5; //5;
            coolDownNow = 0;
            requireAP = 2; //2;
            nonTarget = false;
            range = 2;
            radius = 1;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new RangeAoECastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null)
            {
                foreach (var n in UtilityService.GetHexesRadius(requestData.TargetHex, radius))
                {
                    if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                        AttackService.SetDamage(requestData.Caster, n.HERO, 200, Consts.DamageType.Magic);
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
                range += 1;
                radius += 1;
                stats.range += 1;
                stats.radius += 1;
                return true;
            }
            return false;
        }
    }
}
