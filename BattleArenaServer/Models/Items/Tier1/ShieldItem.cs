namespace BattleArenaServer.Models.Items.Tier1
{
    public class ShieldItem : Item
    {
        int armor = 1;
        public ShieldItem()
        {
            Name = "Shield";
            Amount = 1;
            Cost = 10;
            Description = $"+{armor} к броне";
            SellCost = 5;
        }

        public override void ApplyEffect(Hero hero)
        {
            hero.Armor += armor;
        }

        public override void RemoveEffect(Hero hero)
        {
            hero.Armor -= armor;
        }
    }
}
