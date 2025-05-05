using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.Knight
{
    public class SecondBreathSkill : Skill
    {
        int heal = 150;
        int armor = 3;
        public SecondBreathSkill()
        {
            name = "Second Breath";
            title = $"Восстанавливает владельцу {heal} ХП и дает {armor} доп. брони";
            titleUpg = "-2 к перезарядке, +2 к доп. броне";
            coolDown = 6;
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
                requestData.Caster.SpendAP(requireAP);
                requestData.Caster.Heal(heal);
                coolDownNow = coolDown;

                ArmorBuff buffArmor = new ArmorBuff(requestData.Caster.Id, armor, 2);
                requestData.Caster.AddEffect(buffArmor);

                return true;
            }

            return false;
        }

        public override bool UpgradeSkill()
        {
            if (!upgraded)
            {
                upgraded = true;
                coolDown -= 2;
                stats.coolDown -= 2;
                armor += 2;
                title = $"Восстанавливает владельцу {heal} ХП и дает {armor} доп. брони";
                return true;
            }
            return false;
        }
    }
}
