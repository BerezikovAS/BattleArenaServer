using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.FairySkills
{
    public class RebirthSkill : Skill
    {
        int decreaseMaxHP = 175;
        public RebirthSkill()
        {
            name = "Rebirth";
            title = $"Магическое перерождение отнимает у Вас {decreaseMaxHP} Макс. ХП и полностью восстанавливает здоровье.";
            titleUpg = "-2 к перезарядке, -1 ОД";
            coolDown = 5;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = true;
            area = Consts.SpellArea.NonTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new NontargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.CasterHex != null)
            {
                requestData.Caster.SpendAP(requireAP);
                requestData.Caster.MaxHP -= decreaseMaxHP;

                if (requestData.Caster.MaxHP <= 0)
                    requestData.Caster.MaxHP = 1;

                requestData.Caster.Heal(requestData.Caster.MaxHP);
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
                requireAP -= 1;
                coolDown -= 2;
                stats.coolDown -= 2;
                return true;
            }
            return false;
        }
    }
}
