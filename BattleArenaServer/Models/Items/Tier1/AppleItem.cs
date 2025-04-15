namespace BattleArenaServer.Models.Items.Tier1
{
    public class AppleItem : Item
    {
        int hp = 80;
        public AppleItem()
        {
            Name = "Apple";
            Amount = 1;
            Cost = 10;
            Description = $"+{hp} к здоровью";
            Level = 1;
            SellCost = 5;
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
