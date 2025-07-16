using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Models.Items.Tier1;
using BattleArenaServer.Models.Items.Tier2;
using BattleArenaServer.Models.Items.Tier3;
using BattleArenaServer.Models.Items.Tier4;

namespace BattleArenaServer.Services
{
    public class ShopService : IShop
    {
        GameData gameData;

        public ShopService(GameData gameData)
        {
            this.gameData = gameData;
        }
        public void FillItems()
        {
            var blueShop = gameData._blueShop;
            var redShop = gameData._redShop;

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
            blueShop.Add(new HolyCleansingItem()); redShop.Add(new HolyCleansingItem());
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

        public List<Item> GetShopItems()
        {
            if (gameData.activeTeam == "red")
                return gameData._redShop;
            return gameData._blueShop;
        }

        public void BuyItem(Hero hero, Item item)
        {
            int teamCoins = hero.Team == "red" ? gameData.userTeamBindings.RedCoins : gameData.userTeamBindings.BlueCoins;
            if (item.Amount < 1 || teamCoins < item.Cost || hero.Items.Count() >= 3)
                return;

            item.Amount--;
            if (hero.Team == "red")
                gameData.userTeamBindings.RedCoins -= item.Cost;
            else
                gameData.userTeamBindings.BlueCoins -= item.Cost;

            hero.Items.Add(item);
            item.ApplyEffect(hero, gameData);
        }

        public void SellItem(Hero hero, Item item)
        {
            if (!hero.Items.Contains(item))
                return;

            item.Amount++;
            item.RemoveEffect(hero, gameData);
            hero.Items.Remove(item);

            if (hero.Team == "red")
                gameData.userTeamBindings.RedCoins += item.SellCost;
            else
                gameData.userTeamBindings.BlueCoins += item.SellCost;
        }
    }
}
