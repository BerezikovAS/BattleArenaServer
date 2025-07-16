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
            description = $"Сопротивление увеличено на {value}. Атакующий герой дополнительно теряет 1 ОД";
        }

        public override void ApplyEffect(Hero _hero, GameData gameData)
        {
            _hero.StatsEffect.Resist += value;
            _hero.afterReceivedAttack += AfterReceivedAttack;
        }

        public override void RemoveEffect(Hero _hero, GameData gameData)
        {
            _hero.StatsEffect.Resist -= value;
            _hero.afterReceivedAttack -= AfterReceivedAttack;
        }

        public bool AfterReceivedAttack(Hero attacker, Hero defender, int dmg, GameData gameData)
        {
            if (attacker.HP > 0 && attacker.AP > 0)
                attacker.AP -= 1;

            /*if (defender.HP <= 0 || attacker.HP <= 0)
                return false;

            Hex? attackerHex = gameData._hexes.FirstOrDefault(x => x.HERO != null && x.HERO.Id == attacker.Id);
            Hex? defenderHex = gameData._hexes.FirstOrDefault(x => x.HERO != null && x.HERO.Id == defender.Id);

            if (attackerHex == null || defenderHex == null)
                return false;

            List<Hex> availableHexes = new List<Hex>();
            availableHexes = gameData._hexes.FindAll(x => x.IsFree() && x.Distance(attackerHex) > x.Distance(defenderHex) && x.Distance(defenderHex) == 1);
            if (availableHexes.Count() >= 1)
            {
                Random rnd = new Random();
                Hex escapeHex = availableHexes[rnd.Next(availableHexes.Count())];
                AttackService.MoveHero(defender, defenderHex, escapeHex, gameData);
            }*/

            return true;
        }
    }
}
