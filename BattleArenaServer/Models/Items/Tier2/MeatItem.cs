namespace BattleArenaServer.Models.Items.Tier2
{
    public class MeatItem : Item
    {
        int hp = 150;
        public MeatItem()
        {
            Name = "Meat";
            Amount = 1;
            Cost = 25;
            Description = $"+{hp} к здоровью";
            Level = 2;
            SellCost = 12;
        }

        public override void ApplyEffect(Hero hero)
        {
            hero.MaxHP += hp;
            hero.HP += hp;
        }

        public override void RemoveEffect(Hero hero)
        {
            hero.MaxHP -= hp;
            hero.HP -= hp;
            if (hero.HP <= 0)
                hero.HP = 1;
            if (hero.MaxHP <= 0)
                hero.MaxHP = 1;
        }
    }
}
