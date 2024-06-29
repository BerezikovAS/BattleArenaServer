using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.BerserkerSkills
{
    public class BloodRageSkill : Skill
    {
        int extraDmg = 0;
        public BloodRageSkill()
        {
            name = "Blood Rage";
            title = "Ваши атаки стоят всего 1 очко действия, но каждая из них отнимает у Вас 40 ХП.";
            titleUpg = $"+{extraDmg} к урону от атак.";
            coolDown = 3;
            coolDownNow = 0;
            requireAP = 0;
            nonTarget = true;
            area = Consts.SpellArea.NonTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new NontargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (request.startRequest(requestData, this))
            {
                if (requestData.Caster != null)
                {
                    BloodRageBuff bloodRageBuff = new BloodRageBuff(requestData.Caster.Id, extraDmg, 1);
                    requestData.Caster.AddEffect(bloodRageBuff);
                    bloodRageBuff.ApplyEffect(requestData.Caster);

                    requestData.Caster.AP -= requireAP;
                    coolDownNow = coolDown;
                    return true;
                }
            }
            return false;
        }

        public override bool UpgradeSkill()
        {
            if (!upgraded)
            {
                upgraded = true;
                extraDmg = 30;
                extraDmgStr = "+30";
                titleUpg = $"+{extraDmg} к урону от атак.";
                title += "\n" + titleUpg;
                return true;
            }
            return false;
        }
    }
}
