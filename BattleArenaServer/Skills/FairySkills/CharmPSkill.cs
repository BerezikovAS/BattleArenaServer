using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.FairySkills
{
    public class CharmPSkill : PassiveSkill
    {
        int burnAP = 1;
        int decreaseDmg1 = 25;
        int decreaseDmg2 = 40;
        int percentDecrease = 0;
        public CharmPSkill(Hero hero) : base(hero)
        {
            name = "Charm";
            title = $"Враги, атакуя Вас, дополнительно теряют {burnAP} ОД. Если враг не может потерять дополнительные ОД, то входящий урон уменьшается на {decreaseDmg1}%.";
            titleUpg = $"+1 к потере ОД. Уменьшение урона (1 ОД = {decreaseDmg1}%, 2 ОД = {decreaseDmg2}%)";
            hero.beforeReceivedAttack += CharmBeforeReceivedAttack;
            hero.afterReceivedAttack += CharmAfterReceivedAttack;
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
                hero.beforeReceivedAttack -= CharmBeforeReceivedAttack;
                hero.afterReceivedAttack -= CharmAfterReceivedAttack;
                upgraded = true;
                hero.beforeReceivedAttack += CharmBeforeReceivedAttack;
                hero.afterReceivedAttack += CharmAfterReceivedAttack;
                title = $"Враги, атакуя Вас, дополнительно теряют {burnAP} ОД. Если враг не может потерять дополнительные ОД, " +
                    $"то входящий урон уменьшается на {decreaseDmg1}% за 1 недостающее ОД и на {decreaseDmg2}% за 2 ОД.";
                return true;
            }
            return false;
        }

        private bool CharmBeforeReceivedAttack(Hero attacker, Hero defender, int dmg)
        {
            percentDecrease = 0;
            // ОД за саму атаку уже сняли, смотрим можем ли мы еще что-нибудь отнять
            if (attacker.AP <= 0 && !upgraded)
                percentDecrease = decreaseDmg1;
            else if (attacker.AP <= 0 && upgraded)
                percentDecrease = decreaseDmg2;
            else if (attacker.AP >= 1 && !upgraded)
                attacker.AP -= 1;
            else if (attacker.AP == 1 && upgraded)
            {
                attacker.AP -= 1;
                percentDecrease = decreaseDmg1;
            }
            else if (attacker.AP >= 2 && upgraded)
                attacker.AP -= 2;

            defender.modifierAppliedDamage += ModifierAppliedDmg;
            return true;
        }

        private bool CharmAfterReceivedAttack(Hero attacker, Hero defender, int dmg)
        {
            defender.modifierAppliedDamage -= ModifierAppliedDmg;
            return true;
        }

        private int ModifierAppliedDmg(Hero? attacker, Hero defender, int dmg)
        {
            return (int)(-1 * Convert.ToDouble(dmg) * Convert.ToDouble(percentDecrease) / 100);
        }
    }
}
