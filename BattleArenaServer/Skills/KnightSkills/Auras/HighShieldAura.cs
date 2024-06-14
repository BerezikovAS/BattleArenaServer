﻿using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.KnightSkills.Auras
{
    public class HighShieldAura : Aura
    {
        public HighShieldAura()
        {
            Name = "HighShieldAura";
            radius = 1;
            type = Consts.AuraType.Continuous;
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
                return 2;
            return 0;
        }
    }
}
