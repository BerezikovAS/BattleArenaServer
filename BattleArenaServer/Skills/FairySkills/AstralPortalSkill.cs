using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;
using BattleArenaServer.Skills.FairySkills.Obstacles;

namespace BattleArenaServer.Skills.FairySkills
{
    public class AstralPortalSkill : Skill
    {
        int duration = 2;
        public AstralPortalSkill()
        {
            name = "Astral Portal";
            dmg = 100;
            title = $"Размещает астральный портал. Союзники и Вы могут переместится на него мгновенно, а враги наступившие на него получают {dmg} " +
                $"чистого урона и получают безмолвие.";
            titleUpg = "-1 к перезарядке, +40 к урону";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            range = 3;
            nonTarget = false;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Pure;
        }

        public new ISkillCastRequest request => new HexTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null)
            {
                AstralPortalObstacle astralPortalObstacle = new AstralPortalObstacle(requestData.Caster.Id, requestData.TargetHex.ID, duration, requestData.Caster.Team, dmg);
                requestData.TargetHex.SetObstacle(astralPortalObstacle);

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
                coolDown -= 1;
                stats.coolDown -= 1;
                dmg += 40;
                title = $"Размещает астральный портал. Союзники и Вы могут переместится на него мгновенно, а враги наступившие на него получают {dmg} " +
                    $"чистого урона и получают безмолвие.";
                return true;
            }
            return false;
        }
    }
}
