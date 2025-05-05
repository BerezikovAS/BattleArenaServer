using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Effects.Unique;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.GuardianSkills
{
    public class BattleOrderSkill : Skill
    {
        public BattleOrderSkill()
        {
            name = "Battle Order";
            title = $"Боевой приказ заставляет Вас или союзника сражаться не обращая внимания на недуги. Защищает от негативных эффектов.";
            titleUpg = "+1 к дальности, также даёт ускорение.";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            range = 2;
            nonTarget = false;
            area = Consts.SpellArea.AllyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new AllyTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (requestData.Target != null && requestData.Caster != null)
            {
                if (!request.startRequest(requestData, this))
                    return false;

                ImmunUnique immunUnique = new ImmunUnique(requestData.Caster.Id, 0, 3);
                requestData.Target.AddEffect(immunUnique);

                if (upgraded)
                {
                    HasteBuff hasteBuff = new HasteBuff(requestData.Caster.Id, 0, 2);
                    requestData.Target.AddEffect(hasteBuff);
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
                title = $"Боевой приказ заставляет Вас или союзника сражаться не обращая внимания на недуги. Защищает от негативных эффектов и даёт ускорение.";
                return true;
            }
            return false;
        }
    }
}
