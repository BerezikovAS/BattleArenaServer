using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;
using BattleArenaServer.Effects.Buffs;

namespace BattleArenaServer.Skills.Crossbowman
{
    public class EagleEye : Skill
    {
        int extraDamage = 20;
        public EagleEye()
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
        }

        public new ISkillCastRequest request => new NontargetCastRequest();

        public override bool Cast(Hero caster, Hero? target, Hex? targetHex)
        {
            if (request.startRequest(caster, target, targetHex, this))
            {
                if (caster != null)
                {
                    EagleEyeBuff eagleEyeBuff = new EagleEyeBuff(caster.Id, extraDamage, 1);
                    caster.EffectList.Add(eagleEyeBuff);
                    eagleEyeBuff.ApplyEffect(caster);

                    caster.AP -= requireAP;
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
                extraDamage += 30;
                return true;
            }
            return false;
        }
    }
}
