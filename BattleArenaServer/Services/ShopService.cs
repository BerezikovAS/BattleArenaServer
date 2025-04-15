using BattleArenaServer.Models;
using BattleArenaServer.Models.Items.Tier1;
using BattleArenaServer.Models.Items.Tier2;
using BattleArenaServer.Models.Items.Tier3;
using BattleArenaServer.Models.Items.Tier4;

namespace BattleArenaServer.Services
{
    public static class ShopService
    {
        public static void FillItems()
        {
            var blueShop = GameData._blueShop;
            var redShop = GameData._redShop;

            blueShop.Clear();
            redShop.Clear();

            // Level 1 Items
            blueShop.Add(new AppleItem()); redShop.Add(new AppleItem());
            blueShop.Add(new SwordItem()); redShop.Add(new SwordItem());
            blueShop.Add(new ShieldItem()); redShop.Add(new ShieldItem());
            blueShop.Add(new TiaraItem()); redShop.Add(new TiaraItem());
            blueShop.Add(new FeatherItem()); redShop.Add(new FeatherItem());
            blueShop.Add(new GrapplingHookItem()); redShop.Add(new GrapplingHookItem());
            blueShop.Add(new ThrowingAxeItem()); redShop.Add(new ThrowingAxeItem());
            blueShop.Add(new ScrollItem()); redShop.Add(new ScrollItem());

            //Level 2 Items
            blueShop.Add(new MeatItem()); redShop.Add(new MeatItem());
            blueShop.Add(new AxeItem()); redShop.Add(new AxeItem());
            blueShop.Add(new ChestPlateItem()); redShop.Add(new ChestPlateItem());
            blueShop.Add(new CloakItem()); redShop.Add(new CloakItem());
            blueShop.Add(new SpearItem()); redShop.Add(new SpearItem());
            blueShop.Add(new PotionItem()); redShop.Add(new PotionItem());
            blueShop.Add(new BootsItem()); redShop.Add(new BootsItem());
            blueShop.Add(new WatchItem()); redShop.Add(new WatchItem());

            //Level 3 Items
            blueShop.Add(new TitanSlayerItem()); redShop.Add(new TitanSlayerItem());
            blueShop.Add(new BattleBannerItem()); redShop.Add(new BattleBannerItem());
            blueShop.Add(new EchoShardItem()); redShop.Add(new EchoShardItem());
            blueShop.Add(new LongBowItem()); redShop.Add(new LongBowItem());
            blueShop.Add(new HammerItem()); redShop.Add(new HammerItem());
            blueShop.Add(new RingItem()); redShop.Add(new RingItem());
            blueShop.Add(new WhistleItem()); redShop.Add(new WhistleItem());
            blueShop.Add(new CombatShieldItem()); redShop.Add(new CombatShieldItem());

            //Level 4 Items
            blueShop.Add(new RoyalSealItem()); redShop.Add(new RoyalSealItem());
            blueShop.Add(new CauldronItem()); redShop.Add(new CauldronItem());
            blueShop.Add(new VampireFangsItem()); redShop.Add(new VampireFangsItem());
            blueShop.Add(new BubbleItem()); redShop.Add(new BubbleItem());
            blueShop.Add(new ShieldBreakerItem()); redShop.Add(new ShieldBreakerItem());
            blueShop.Add(new ScytheItem()); redShop.Add(new ScytheItem());
            blueShop.Add(new HornItem()); redShop.Add(new HornItem());
            blueShop.Add(new GuardItem()); redShop.Add(new GuardItem());
        }

        public static List<Item> GetShopItems()
        {
            if (GameData.activeTeam == "red")
                return GameData._redShop;
            return GameData._blueShop;
        }

        public static void BuyItem(Hero hero, Item item)
        {
            int teamCoins = hero.Team == "red" ? GameData.userTeamBindings.RedCoins : GameData.userTeamBindings.BlueCoins;
            if (item.Amount < 1 || teamCoins < item.Cost || hero.Items.Count() >= 3)
                return;

            item.Amount--;
            if (hero.Team == "red")
                GameData.userTeamBindings.RedCoins -= item.Cost;
            else
                GameData.userTeamBindings.BlueCoins -= item.Cost;

            hero.Items.Add(item);
            item.ApplyEffect(hero);
        }

        public static void SellItem(Hero hero, Item item)
        {
            if (!hero.Items.Contains(item))
                return;

            item.Amount++;
            item.RemoveEffect(hero);
            hero.Items.Remove(item);

            if (hero.Team == "red")
                GameData.userTeamBindings.RedCoins += item.SellCost;
            else
                GameData.userTeamBindings.BlueCoins += item.SellCost;
        }
    }
}
