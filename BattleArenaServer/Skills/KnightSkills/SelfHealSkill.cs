using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.Knight
{
    public class SelfHealSkill : BodyGuardSkill
    {
        int heal = 200;
        int armor = 3;
        public SelfHealSkill() 
        {
            name = "Self heal";
            title = "Восстанавливает владельцу 200 ХП и дает 3 доп. брони";
            titleUpg = "+75 к лечению, +1 к доп. броне";
            coolDown = 4;
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
                requestData.Caster.AP -= requireAP;
                requestData.Caster.Heal(heal);
                coolDownNow = coolDown;

                ArmorBuff buffArmor = new ArmorBuff(requestData.Caster.Id, armor, 2);
                requestData.Caster.EffectList.Add(buffArmor);
                buffArmor.ApplyEffect(requestData.Caster);

                return true;
            }

            return false;
        }

        public override bool UpgradeSkill()
        {
            if (!upgraded)
            {
                upgraded = true;
                heal += 75;
                armor += 1;
                return true;
            }
            return false;
        }
    }
}
