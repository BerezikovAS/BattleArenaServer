namespace BattleArenaServer.Models.Items.Tier3
{
    public class TitanSlayerItem : Item
    {
        //int dmg = 10;
        int percentDmg = 4;
        public TitanSlayerItem()
        {
            Name = "TitanSlayer";
            Amount = 1;
            Cost = 55;
            Description = $"При атаке, дополнительно наносит {percentDmg}% от макс. ХП врага в качестве физ. урона.";
            Level = 3;
            SellCost = 27;
        }

        public override void ApplyEffect(Hero hero)
        {
            //hero.Dmg += dmg;
            hero.passiveAttackDamage += AttackDelegate;
        }

        public override void RemoveEffect(Hero hero)
        {
            //hero.Dmg -= dmg;
            hero.passiveAttackDamage -= AttackDelegate;
        }

        private int AttackDelegate(Hero attacker, Hero? defender)
        {
            if (defender == null)
                return 0;

            return (int)(Convert.ToDouble(defender.MaxHP) * percentDmg / 100);
        }
    }
}
