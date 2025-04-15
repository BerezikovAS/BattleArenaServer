namespace BattleArenaServer.Models.Items.Tier4
{
    public class RoyalSealItem : Item
    {
        int armorResist = 2;
        int dmg = 20;
        int hp = 150;
        public RoyalSealItem()
        {
            Name = "RoyalSeal";
            Amount = 1;
            Cost = 80;
            Description = $"+{dmg} к урону, +{armorResist} к сопротивлению и броне, +{hp} к здоровью";
            Level = 4;
            SellCost = 40;
        }

        public override void ApplyEffect(Hero hero)
        {
            hero.Resist += armorResist;
            hero.Armor += armorResist;
            hero.Dmg += dmg;
            hero.MaxHP += hp;
            hero.HP += hp;
        }

        public override void RemoveEffect(Hero hero)
        {
            hero.Resist -= armorResist;
            hero.Armor -= armorResist;
            hero.Dmg -= dmg;
            hero.MaxHP -= hp;
            hero.HP -= hp;
            if (hero.HP <= 0)
                hero.HP = 1;
            if (hero.MaxHP <= 0)
                hero.MaxHP = 1;
        }
    }
}
