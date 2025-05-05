using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Effects.Buffs
{
    public class RiposteBuff : Effect
    {
        int percentDmg = 60;
        public RiposteBuff(int _idCaster, int _value, int _duration, int _percentDmg)
        {
            Name = "Riposte";
            type = Consts.StatusEffect.Buff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            percentDmg = _percentDmg;
            description = $"+{value} брони. {percentDmg}% урона от атак, наносится атакующим Вас врагам в ближнем бою.";
        }

        public override void ApplyEffect(Hero _hero)
        {
            _hero.Armor += value;
            _hero.afterReceivedAttack += AfterReceiveAttackDelegate;
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.Armor -= value;
            _hero.afterReceivedAttack -= AfterReceiveAttackDelegate;
        }

        private bool AfterReceiveAttackDelegate(Hero attacker, Hero defender, int dmg)
        {
            Hex? attackerHex = GameData._hexes.FirstOrDefault(x => x.HERO != null && x.HERO.Id == attacker.Id);
            Hex? defenderHex = GameData._hexes.FirstOrDefault(x => x.HERO != null && x.HERO.Id == defender.Id);

            if (attackerHex != null && defenderHex != null && attackerHex.Distance(defenderHex) == 1)
            {
                int dealDmg = (int)(Convert.ToDouble(defender.Dmg * percentDmg) / 100);
                AttackService.SetDamage(defender, attacker, dealDmg, Consts.DamageType.Physical);
                return true;
            }
            return false;
        }
    }
}
