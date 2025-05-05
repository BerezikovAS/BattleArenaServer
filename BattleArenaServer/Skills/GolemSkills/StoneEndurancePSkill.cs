using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.GolemSkills
{
    public class StoneEndurancePSkill : PassiveSkill
    {
        int percentHeal = 20;
        int dmgReceived = 0;
        public StoneEndurancePSkill(Hero hero) : base(hero)
        {
            name = "Stone Endurance";
            title = $"В начале хода восстанавливает себе ХП в размере {percentHeal}% от полученного урона в предыдущий ход.";
            titleUpg = "+10% к восстановлению";
            hero.afterReceiveDmg += AfterReceiveDmg;
        }

        public override bool Cast(RequestData requestData)
        {
            return false;
        }

        public override void refreshEffect()
        {
            int heal = (int)(Convert.ToDouble(percentHeal * dmgReceived) / 100);
            hero.Heal(heal);
            dmgReceived = 0;
        }

        public override bool UpgradeSkill()
        {
            upgraded = true;
            hero.afterReceiveDmg -= AfterReceiveDmg;
            percentHeal += 10;
            hero.afterReceiveDmg += AfterReceiveDmg;
            title = $"В начале хода восстанавливает себе ХП в размере {percentHeal}% от полученного урона в предыдущий ход.";
            return true;
        }

        public void AfterReceiveDmg(Hero hero, Hero? attacker, int dmg, Consts.DamageType dmgType)
        {
            dmgReceived += dmg;
        }
    }
}
