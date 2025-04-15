using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.Crossbowman
{
    public class LongShotPSkill : PassiveSkill
    {
        double extraDmgPercent = 0.12;
        public LongShotPSkill(Hero hero) : base(hero)
        {
            name = "Long Shot";
            title = $"Чем дальше цель атаки, тем больше урона она получит. +{Math.Round(extraDmgPercent * 100)}% к урону за каждую клетку между Вами и целью.";
            titleUpg = "+7% к бонусу урона за клетку.";
            hero.passiveAttackDamage += LongShot;
        }

        public override bool Cast(RequestData requestData)
        {
            return false;
        }

        public override void refreshEffect()
        {
        }

        public override bool UpgradeSkill()
        {
            if (!upgraded)
            {
                upgraded = true;
                hero.passiveAttackDamage -= LongShot;
                extraDmgPercent += 0.07;
                hero.passiveAttackDamage += LongShot;
                title = $"Чем дальше цель атаки, тем больше урона она получит. +{Math.Round(extraDmgPercent * 100)}% к урону за каждую клетку между Вами и целью.";
                return true;
            }
            return false;
        }
        private int LongShot(Hero attacker, Hero? defender)
        {
            if (defender == null)
                return 0;

            Hex? attackerHex = GameData._hexes.FirstOrDefault(x => x.HERO?.Id == attacker.Id);
            Hex? defenderHex = GameData._hexes.FirstOrDefault(x => x.HERO?.Id == defender.Id);

            if (attackerHex != null && defenderHex != null)
            {
                double extraDmg = attacker.Dmg * extraDmgPercent * (attackerHex.Distance(defenderHex) - 1);
                return (int)(Math.Round(extraDmg));
            }
            return 0;
        }
    }
}
