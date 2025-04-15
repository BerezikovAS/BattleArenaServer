namespace BattleArenaServer.Models.Items.Tier2
{
    public class SpearItem : Item
    {
        int armorPiercing = 2;
        public SpearItem()
        {
            Name = "Spear";
            Amount = 1;
            Cost = 25;
            Description = $"Атаки и способности игнорируют {armorPiercing} брони цели.";
            Level = 2;
            SellCost = 12;
        }

        public override void ApplyEffect(Hero hero)
        {
            hero.armorPiercing += ArmorPiercingDelegate;
        }

        public override void RemoveEffect(Hero hero)
        {
            hero.armorPiercing -= ArmorPiercingDelegate;
        }

        private int ArmorPiercingDelegate(Hero attacker, Hero defender)
        {
            return armorPiercing;
        }
    }
}
