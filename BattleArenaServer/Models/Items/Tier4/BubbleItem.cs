using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Models.Items.Auras;

namespace BattleArenaServer.Models.Items.Tier4
{
    public class BubbleItem : Item
    {
        int armorResist = 1;
        Aura aura;
        public BubbleItem()
        {
            Name = "Bubble";
            Amount = 1;
            Cost = 80;
            Description = $"+{armorResist} брони и сопротивления. В начале хода даёт эффект, который защищает от первого получения урона.";
            Level = 4;
            SellCost = 40;
            aura = new BubbleAura();
        }

        public override void ApplyEffect(Hero hero)
        {
            hero.Resist += armorResist;
            hero.Armor += armorResist;

            BubbleBuff bubbleBuff = new BubbleBuff(hero.Id, 0, 99);
            hero.AddEffect(bubbleBuff);

            hero.AuraList.Add(aura);
        }

        public override void RemoveEffect(Hero hero)
        {
            hero.Resist -= armorResist;
            hero.Armor -= armorResist;

            hero.AuraList.Remove(aura);
        }
    }
}
