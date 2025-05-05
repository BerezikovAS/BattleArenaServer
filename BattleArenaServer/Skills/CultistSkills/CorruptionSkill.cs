using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;
using BattleArenaServer.Skills.CultistSkills.Obstacles;

namespace BattleArenaServer.Skills.CultistSkills
{
    public class CorruptionSkill : Skill
    {
        int duration = 2;
        public CorruptionSkill()
        {
            name = "Corruption";
            dmg = 25;
            title = $"Наводит порчу на поверхность. Враги переместившиеся на неё теряют {dmg} ХП, а Вы восполняете такое же количество.";
            titleUpg = "+10 к урону. +1 к времени действия порчи.";
            coolDown = 5;
            coolDownNow = 0;
            requireAP = 1;
            radius = 1;
            range = 2;
            nonTarget = false;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Pure;
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
                    CorruptionObstacle corruptionObstacle = new CorruptionObstacle(requestData.Caster.Id, n.ID, duration, requestData.Caster.Team, dmg);
                    n.AddSurface(corruptionObstacle);
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
                duration += 1;
                dmg += 10;
                title = $"Наводит порчу на поверхность. Враги переместившиеся на неё теряют {dmg} ХП, а Вы восполняете такое же количество.";
                return true;
            }
            return false;
        }
    }
}
