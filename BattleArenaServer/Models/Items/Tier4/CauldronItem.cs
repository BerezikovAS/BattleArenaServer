using BattleArenaServer.Models.Items.Auras;

namespace BattleArenaServer.Models.Items.Tier4
{
    public class CauldronItem : Item
    {
        int dmg = 70;
        int percentLoss = 10;
        Aura aura;
        public CauldronItem()
        {
            Name = "Cauldron";
            Amount = 1;
            Cost = 80;
            Description = $"Ведьмовской котёл источает свои зловония и наносит {dmg} маг. урона и накладывает отравление на врагов вокруг в конце Вашего хода.";
            Level = 4;
            SellCost = 40;
            aura = new CauldronAura(dmg, percentLoss);
        }

        public override void ApplyEffect(Hero hero)
        {
            hero.AuraList.Add(aura);
        }

        public override void RemoveEffect(Hero hero)
        {
            hero.AuraList.Remove(aura);
        }
    }
}
