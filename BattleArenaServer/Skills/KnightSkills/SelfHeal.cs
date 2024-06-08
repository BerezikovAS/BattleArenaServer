using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.Knight
{
    public class SelfHeal : Skill
    {
        int heal = 200;
        int armor = 3;
        public SelfHeal() 
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

        public override bool Cast(Hero caster, Hero? target, Hex? targetHex)
        {
            if(request.startRequest(caster, target, targetHex, this))
            {
                if(caster != null)
                {
                    caster.AP -= requireAP;
                    caster.Heal(heal);
                    coolDownNow = coolDown;

                    ArmorBuff buffArmor = new ArmorBuff(caster.Id, armor, 2);
                    caster.EffectList.Add(buffArmor);
                    buffArmor.ApplyEffect(caster);

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
                heal += 75;
                armor += 1;
                return true;
            }
            return false;
        }
    }
}
