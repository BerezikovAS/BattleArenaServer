namespace BattleArenaServer.Models.Items.Tier4
{
    public class VampireFangsItem : Item
    {
        int extraHP = 100;
        int percentLifeSteal = 80;
        public VampireFangsItem()
        {
            Name = "VampireFangs";
            Amount = 1;
            Cost = 80;
            Description = $"+{extraHP} ХП. Восстанавливает здоровье в размере {percentLifeSteal}% от нанесенного урона от атак.";
            Level = 4;
            SellCost = 40;
        }

        public override void ApplyEffect(Hero hero)
        {
            hero.MaxHP += extraHP;
            hero.HP += extraHP;
            hero.beforeAttack += BeforeAttackDelegate;
            hero.afterAttack += AfterAttackDelegate;
        }

        public override void RemoveEffect(Hero hero)
        {
            hero.MaxHP -= extraHP;
            hero.HP -= extraHP;
            if (hero.HP <= 0)
                hero.HP = 1;
            if (hero.MaxHP <= 0)
                hero.MaxHP = 1;
            hero.beforeAttack -= BeforeAttackDelegate;
            hero.afterAttack -= AfterAttackDelegate;
        }

        private bool BeforeAttackDelegate(Hero attacker, Hero defender, int dmg)
        {
            attacker.DamageDealed = 0;
            return true;
        }

        private bool AfterAttackDelegate(Hero attacker, Hero defender, int dmg, Consts.DamageType dmgType)
        {
            if (attacker.HP <= 0)
                return false;

            int heal = (int)(Convert.ToDouble(attacker.DamageDealed) * percentLifeSteal / 100);
            attacker.Heal(heal);
            return true;
        }
    }
}
