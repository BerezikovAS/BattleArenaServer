using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Skills.GuardianSkills.Auras
{
    public class EncourageAura : Aura
    {
        int extraArmor = 0;
        int extraDmg = 0;
        public EncourageAura(int _extraArmor, int _extraDmg)
        {
            Name = "Encourage";
            radius = 1;
            type = Consts.AuraType.Continuous;
            extraArmor = _extraArmor;
            extraDmg = _extraDmg;
        }

        public override void ApplyEffect(Hero source, Hero target)
        {
            if (source.Team == target.Team)
            {
                target.passiveArmor += EncourageArmor;
                target.passiveAttackDamage += EncourageDmg;
            }
        }

        public override void CancelEffect(Hero source)
        {
            foreach (var hero in GameData._heroes)
            {
                hero.passiveArmor -= EncourageArmor;
                hero.passiveAttackDamage -= EncourageDmg;
            }
        }

        private int EncourageArmor(Hero? attacker, Hero defender)
        {
            int addArmor = 0;
            Hex? defenderHex = GameData._hexes.FirstOrDefault(x => x.HERO?.Id == defender.Id);
            if (defenderHex == null)
                return 0;

            foreach (var hex in UtilityService.GetHexesRadius(defenderHex, 1))
            {
                if (hex.HERO != null && hex.HERO.Team != defender.Team)
                    addArmor++;
            }

            return extraArmor * addArmor;
        }

        private int EncourageDmg(Hero attacker, Hero? defender)
        {
            int addDmg = 0;
            Hex? attackerHex = GameData._hexes.FirstOrDefault(x => x.HERO?.Id == attacker.Id);
            if (attackerHex == null)
                return 0;

            foreach (var hex in UtilityService.GetHexesRadius(attackerHex, 1))
            {
                if (hex.HERO != null && hex.HERO.Team != attacker.Team)
                    addDmg++;
            }

            return extraDmg * addDmg;
        }
    }
}
