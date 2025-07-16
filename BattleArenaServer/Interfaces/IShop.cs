using BattleArenaServer.Models;

namespace BattleArenaServer.Interfaces
{
    public interface IShop
    {
        public void FillItems();
        public List<Item> GetShopItems();
        public void BuyItem(Hero hero, Item item);
        public void SellItem(Hero hero, Item item);
    }
}
