using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.DruidSkills
{
    public class AdaptiveArmorPSkill : PassiveSkill
    {
        int addingArmRes = 2;
        int extraArmor = 0;
        int extraResist = 0;
        public AdaptiveArmorPSkill(Hero hero) : base(hero)
        {
            name = "Adaptive Armor";
            title = $"Получая урон, Вы увеличиваете своё сопротивление ему. +{addingArmRes} брони/сопротивления.";
            titleUpg = "+ к увеличению брони/сопротивления";
            skillType = Consts.SkillType.Passive;
            hero.afterReceiveDmg += AfterReceiveDmgDelegate;
        }

        public override bool Cast(RequestData requestData)
        {
            return false;
        }

        public override void refreshEffect()
        {
            hero.Armor -= extraArmor;
            hero.Resist -= extraResist;

            extraArmor = extraResist = 0;
        }

        public override bool UpgradeSkill()
        {
            if (!upgraded)
            {
                upgraded = true;
                hero.afterReceiveDmg -= AfterReceiveDmgDelegate;
                addingArmRes += 2;
                hero.afterReceiveDmg += AfterReceiveDmgDelegate;
                title = $"Получая урон, Вы увеличиваете своё сопротивление ему. +{addingArmRes} брони/сопротивления.";
                return true;
            }
            return false;
        }

        private void AfterReceiveDmgDelegate(Hero defender, Hero? attacker, int dmg, Consts.DamageType dmgType)
        {
            if (dmgType == Consts.DamageType.Physical)
            {
                extraArmor += addingArmRes;
                hero.Armor += addingArmRes;
            }
            if (dmgType == Consts.DamageType.Magic)
            {
                extraResist += addingArmRes;
                hero.Resist += addingArmRes;
            }
        }
    }
}
