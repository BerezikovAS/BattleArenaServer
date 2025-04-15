using BattleArenaServer.Models.Items.Auras;
using BattleArenaServer.Services;

namespace BattleArenaServer.Models.Items.Tier3
{
    public class BattleBannerItem : Item
    {
        int armorResist = 1;
        int dmg = 10;
        Aura aura;
        public BattleBannerItem()
        {
            Name = "BattleBanner";
            Amount = 1;
            Cost = 55;
            Description = $"Аура боевого знамени даёт +{dmg} к урону, +{armorResist} к сопротивлению и броне в радиусе 2-х клеток";
            Level = 3;
            SellCost = 27;
            aura = new BattleBannerAura(armorResist, dmg, 2);
        }

        public override void ApplyEffect(Hero hero)
        {
            hero.AuraList.Add(aura);
            AttackService.ContinuousAuraAction();
        }

        public override void RemoveEffect(Hero hero)
        {
            hero.AuraList.Remove(aura);
            AttackService.ContinuousAuraAction();
        }
    }
}
