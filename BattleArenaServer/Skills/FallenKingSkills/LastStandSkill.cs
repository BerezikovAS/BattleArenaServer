using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.FallenKingSkills
{
    public class LastStandSkill : Skill
    {
        int afterHeal = 125;
        public LastStandSkill()
        {
            name = "Last Stand";
            title = $"Вы или союзник получаете заклятие, которое не даёт здоровью опуститься ниже 1. По окончанию действия излечивает на {afterHeal} ХП.";
            titleUpg = "+1 к дальности, +50 к лечению";
            coolDown = 5;
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

                LastStandBuff lastStandBuff = new LastStandBuff(requestData.Caster.Id, afterHeal, 2);
                requestData.Target.AddEffect(lastStandBuff);

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
                afterHeal += 50;
                range += 1;
                stats.range += 1;
                title = $"Вы или союзник получаете заклятие, которое не даёт здоровью опуститься ниже 1. По окончанию действия излечивает на {afterHeal} ХП.";
                return true;
            }
            return false;
        }
    }
}
