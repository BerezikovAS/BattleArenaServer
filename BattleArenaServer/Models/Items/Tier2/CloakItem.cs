namespace BattleArenaServer.Models.Items.Tier2
{
    public class CloakItem : Item
    {
        int resist = 2;
        public CloakItem()
        {
            Name = "Cloak";
            Amount = 1;
            Cost = 25;
            Description = $"+{resist} к сопротивлению";
            Level = 2;
            SellCost = 12;
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
