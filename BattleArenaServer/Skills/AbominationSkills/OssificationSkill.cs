using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;
using BattleArenaServer.Interfaces;

namespace BattleArenaServer.Skills.AbominationSkills
{
    public class OssificationSkill : Skill
    {
        int maxHPreduction = 60;
        public OssificationSkill()
        {
            name = "Ossification";
            title = $"Уменьшает максимальный запас ХП на {maxHPreduction}, чтобы навсегда получить +1 к броне и сопротивлению и +7 к урону.";
            titleUpg = "ХП уменьшается на 30 и способность не тратит ОД.";
            coolDown = 1;
            coolDownNow = 0;
            requireAP = 1;
            nonTarget = true;
            area = Consts.SpellArea.NonTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new NontargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null)
            {
                requestData.Caster.MaxHP -= maxHPreduction;
                if (requestData.Caster.HP > requestData.Caster.MaxHP)
                    requestData.Caster.HP = requestData.Caster.MaxHP;

                requestData.Caster.Armor += 1;
                requestData.Caster.Resist += 1;
                requestData.Caster.Dmg += 7;

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
                requireAP = 0;
                maxHPreduction = 30;
                title = $"Уменьшает максимальный запас ХП на {maxHPreduction}, чтобы навсегда получить +1 к броне и сопротивлению и +7 к урону.";
                return true;
            }
            return false;
        }
    }
}
