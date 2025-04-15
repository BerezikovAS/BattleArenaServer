namespace BattleArenaServer.Models.Items.Tier3
{
    public class LongBowItem : Item
    {
        int extraAttackRange = 1;
        int percentDmg = 20;
        public LongBowItem()
        {
            Name = "LongBow";
            Amount = 1;
            Cost = 55;
            Description = $"(Только герои дальнего боя!) +{extraAttackRange} к дальности атаки. При атаке на максимальную дальность +{percentDmg}% к урону.";
            Level = 3;
            SellCost = 27;
        }

        public override void ApplyEffect(Hero hero)
        {
            if (hero.AttackRadius > 1)
            {
                hero.AttackRadius += extraAttackRange;
                hero.passiveAttackDamage += AttackDelegate;
            }
        }

        public override void RemoveEffect(Hero hero)
        {
            if (hero.AttackRadius > 1)
            {
                hero.AttackRadius -= extraAttackRange;
                hero.passiveAttackDamage -= AttackDelegate;
            }
        }

        private int AttackDelegate(Hero attacker, Hero? defender)
        {
            if (defender == null)
                return 0;

            Hex? attackerHex = GameData._hexes.FirstOrDefault(x => x.HERO != null && x.HERO.Id == attacker.Id);
            Hex? defenderHex = GameData._hexes.FirstOrDefault(x => x.HERO != null && x.HERO.Id == defender.Id);

            if (attackerHex != null && defenderHex != null)
            {
                if (attackerHex.Distance(defenderHex) >= attacker.AttackRadius)
                    return (int)(Convert.ToDouble(attacker.Dmg) * percentDmg / 100);
            }

            return 0;
        }
    }
}
