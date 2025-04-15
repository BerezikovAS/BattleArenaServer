namespace BattleArenaServer.Models.Items.Tier1
{
    public class SwordItem : Item
    {
        int dmg = 10;
        public SwordItem()
        {
            Name = "Sword";
            Amount = 1;
            Cost = 10;
            Description = $"+{dmg} к урону от атак";
            SellCost = 5;
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
