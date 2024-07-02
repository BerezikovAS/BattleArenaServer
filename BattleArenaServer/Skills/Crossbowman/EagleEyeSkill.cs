using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.Crossbowman
{
    public class EagleEyeSkill : Skill
    {
        int extraDamage = 30;
        public EagleEyeSkill()
        {
            name = "EagleEye";
            title = $"Увеличивает дальность атаки на 1 и урон на {extraDamage}.";
            titleUpg = "+30 к дополнительному урону.";
            coolDown = 1;
            coolDownNow = 0;
            requireAP = 1;
            nonTarget = true;
            area = Consts.SpellArea.NonTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            extraDmgStr = "+30";
        }

        public new ISkillCastRequest request => new NontargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null)
            {
                EagleEyeBuff eagleEyeBuff = new EagleEyeBuff(requestData.Caster.Id, extraDamage, 1);
                requestData.Caster.AddEffect(eagleEyeBuff);
                eagleEyeBuff.ApplyEffect(requestData.Caster);

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
                extraDamage += 30;
                extraDmgStr = "+60";
                title = $"Увеличивает дальность атаки на 1 и урон на {extraDamage}.";
                return true;
            }
            return false;
        }
    }
}
