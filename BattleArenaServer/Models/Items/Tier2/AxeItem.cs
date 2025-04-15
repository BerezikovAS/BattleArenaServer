namespace BattleArenaServer.Models.Items.Tier2
{
    public class AxeItem : Item
    {
        int dmg = 20;
        public AxeItem()
        {
            Name = "Axe";
            Amount = 1;
            Cost = 25;
            Description = $"+{dmg} к урону от атак";
            Level = 2;
            SellCost = 12;
        }

        public override void ApplyEffect(Hero hero)
        {
            hero.Dmg += dmg;
        }

        public override void RemoveEffect(Hero hero)
        {
            hero.Dmg -= dmg;
        }
    }
}
