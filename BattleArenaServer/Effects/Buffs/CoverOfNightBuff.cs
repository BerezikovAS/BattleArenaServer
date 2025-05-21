using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Effects.Buffs
{
    public class CoverOfNightBuff : Effect
    {
        public CoverOfNightBuff(int _idCaster, int _value, int _duration)
        {
            Name = "CoverOfNight";
            type = Consts.StatusEffect.Buff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = $"Сопротивление увеличено на {value}. При атаке по себе перемещается в сторону от атакующего";
        }

        public override void ApplyEffect(Hero _hero)
        {
            _hero.StatsEffect.Resist += value;
            _hero.afterReceivedAttack += AfterReceivedAttack;
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.StatsEffect.Resist -= value;
            _hero.afterReceivedAttack -= AfterReceivedAttack;
        }

        public bool AfterReceivedAttack(Hero attacker, Hero defender, int dmg)
        {
            if (defender.HP <= 0 || attacker.HP <= 0)
                return false;

            Hex? attackerHex = GameData._hexes.FirstOrDefault(x => x.HERO != null && x.HERO.Id == attacker.Id);
            Hex? defenderHex = GameData._hexes.FirstOrDefault(x => x.HERO != null && x.HERO.Id == defender.Id);

            if (attackerHex == null || defenderHex == null)
                return false;

            List<Hex> availableHexes = new List<Hex>();
            availableHexes = GameData._hexes.FindAll(x => x.IsFree() && x.Distance(attackerHex) > x.Distance(defenderHex) && x.Distance(defenderHex) == 1);
            if (availableHexes.Count() >= 1)
            {
                Random rnd = new Random();
                Hex escapeHex = availableHexes[rnd.Next(availableHexes.Count())];
                AttackService.MoveHero(defender, defenderHex, escapeHex);
            }

            return true;
        }
    }
}
