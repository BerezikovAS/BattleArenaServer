using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Effects.Debuffs
{
    public class BleedingDebuff : Effect
    {
        public BleedingDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "Bleeding";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            effectType = Consts.EffectType.EndTurn;
            description = "Теряет " + value + "ХП в конце своего хода.";
        }

        public override void ApplyEffect(Hero defender, GameData gameData)
        {
            Hero? attacker = gameData._heroes.FirstOrDefault(x => x.Id == idCaster);
            AttackService.SetDamage(attacker, defender, value, Consts.DamageType.Pure, gameData);
        }

        public override void RemoveEffect(Hero _hero, GameData gameData)
        {

        }
    }
}
