using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.KnightSkills.Auras
{
    public class HighShieldAura : Aura
    {
        bool upgraded = false;
        public HighShieldAura(bool upgraded)
        {
            Name = "HighShieldAura";
            radius = 1;
            type = Consts.AuraType.Continuous;
            this.upgraded = upgraded;
        }

        public override void ApplyEffect(Hero source, Hero target)
        {
            if (source.Team == target.Team)
                target.passiveArmor += HighShield;
        }

        public override void CancelEffect(Hero source)
        {
            foreach (var hero in GameData._heroes)
            {
                hero.passiveArmor -= HighShield;
            }
        }

        private int HighShield(Hero? attacker, Hero defender)
        {
            Hex? attackerHex = GameData._hexes.FirstOrDefault(x => x.HERO?.Id == attacker.Id);
            Hex? defenderHex = GameData._hexes.FirstOrDefault(x => x.HERO?.Id == defender.Id);

            if (attackerHex != null && defenderHex != null && attackerHex.Distance(defenderHex) > 1)
            {
                if (upgraded)
                    return 6;
                return 3;
            }
            return 0;
        }
    }
}
