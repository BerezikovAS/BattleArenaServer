using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.AeroturgSkills
{
    public class AirFormSkill : Skill
    {
        int percentReduce = 50;
        public AirFormSkill()
        {
            name = "Air Form";
            title = $"Превращает свое тело в газообразную форму и получает только {percentReduce}% урона.";
            titleUpg = "+25% к поглощению урона.";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            nonTarget = true;
            area = Consts.SpellArea.NonTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new NontargetCastRequest();

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (!request.startRequest(requestData, this, gameData))
                return false;

            if (requestData.Caster != null)
            {
                AirFormBuff airFormBuff = new AirFormBuff(requestData.Caster.Id, percentReduce, 2);
                requestData.Caster.AddEffect(airFormBuff, gameData);

                requestData.Caster.SpendAP(requireAP, gameData);
                coolDownNow = coolDown;
                return true;
            }

            return false;
        }

        public override bool UpgradeSkill(GameData gameData)
        {
            if (!upgraded)
            {
                upgraded = true;
                percentReduce += 25;
                title = $"Превращает свое тело в газообразную форму и получает только {percentReduce}% урона.";
                return true;
            }
            return false;
        }
    }
}
