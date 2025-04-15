namespace BattleArenaServer.Models.Items.Tier2
{
    public class ChestPlateItem : Item
    {
        int armor = 2;
        public ChestPlateItem()
        {
            Name = "ChestPlate";
            Amount = 1;
            Cost = 25;
            Description = $"+{armor} к броне";
            Level = 2;
            SellCost = 12;
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
