using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.SnowQueenSkills
{
    public class DeepFreezeSkill : Skill
    {
        int heal = 50;
        int extraHeal = 25;
        public DeepFreezeSkill()
        {
            name = "Deep Freeze";
            title = $"Вы или союзник тратит все свои ОД и восстанавливает ХП = {heal} + {extraHeal} * ОД, а также становится невосприимчив к урону до следующего своего хода.";
            titleUpg = "+25 к базовому лечению, +10 к лечению за ОД";
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

                int usedAP = requestData.Target.AP;
                requestData.Target.Heal(heal + usedAP * extraHeal);
                requestData.Target.AP -= usedAP;

                DeepFreezeBuff deepFreezeBuff = new DeepFreezeBuff(requestData.Caster.Id, 0, 1);
                requestData.Target.AddEffect(deepFreezeBuff);
                deepFreezeBuff.ApplyEffect(requestData.Target);

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
                heal += 25;
                extraHeal += 10;
                title = $"Вы или союзник тратит все свои ОД и восстанавливает ХП = {heal} + {extraHeal} * ОД, а также становится невосприимчив к урону до следующего своего хода.";
                return true;
            }
            return false;
        }
    }
}
