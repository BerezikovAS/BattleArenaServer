using BattleArenaServer.Skills;

namespace BattleArenaServer.Models
{
    public class Item
    {
        public string Name { get; set; } = "";
        public int Cost { get; set; } = 1;
        public int SellCost { get; set; } = 1;
        public int Amount { get; set; } = 1;
        public string Description { get; set; } = "";
        public int Level { get; set; } = 1;
        public int RequireAP { get; set; } = 0;
        public int Cooldown { get; set; } = 0;
        public int CooldownNow { get; set; } = 0;
        public Consts.ItemType ItemType { get; set; } = Consts.ItemType.Passive;
        public Skill Skill { get; set; } = new EmptySkill();

        public Item() { }

        public Item(string name, int cost, int amount, string descr)
        {
            Name = name;
            Cost = cost;
            Amount = amount;
            Description = descr;
        }

        public virtual void ApplyEffect(Hero hero)
        {

        }

        public virtual void RemoveEffect(Hero hero)
        {

        }

        public virtual void CastItem(RequestData requestData)
        {

        }
    }
}
