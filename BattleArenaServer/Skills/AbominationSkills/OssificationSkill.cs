using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;
using BattleArenaServer.Interfaces;

namespace BattleArenaServer.Skills.AbominationSkills
{
    public class OssificationSkill : Skill
    {
        int maxHPreduction = 80;
        int extraDef = 1;
        int extraDmg = 7;
        public OssificationSkill()
        {
            name = "Ossification";
            title = $"Уменьшает максимальный запас ХП на {maxHPreduction}, чтобы навсегда получить +{extraDef} к броне и сопротивлению и +{extraDmg} к урону.";
            titleUpg = "Уменьшает ХП на 140, но даёт двойной бонус к статам.";
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

                requestData.Caster.Armor += extraDef;
                requestData.Caster.Resist += extraDef;
                requestData.Caster.Dmg += extraDmg;

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
                maxHPreduction += 60;
                extraDef += 1;
                extraDmg += 7;
                title = $"Уменьшает максимальный запас ХП на {maxHPreduction}, чтобы навсегда получить +{extraDef} к броне и сопротивлению и +{extraDmg} к урону.";
                return true;
            }
            return false;
        }
    }
}
