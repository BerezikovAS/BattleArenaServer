namespace BattleArenaServer.Models.Items.Tier1
{
    public class TiaraItem : Item
    {
        int resist = 1;
        public TiaraItem()
        {
            Name = "Tiara";
            Amount = 1;
            Cost = 10;
            Description = $"+{resist} к сопротивлению";
            SellCost = 5;
        }

        public override void ApplyEffect(Hero hero)
        {
            hero.Resist += resist;
        }

        public override void RemoveEffect(Hero hero)
        {
            hero.Resist -= resist;
        }
    }
}
