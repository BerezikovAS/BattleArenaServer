using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Effects.Debuffs
{
    public class BleedingDebuff : Effect
    {
        public BleedingDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "Bleeding";
            type = "debuff";
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            effectType = Consts.EffectType.EndTurn;
            description = "Теряет " + value + "ХП в конце своего хода.";
        }

        public override void ApplyEffect(Hero defender)
        {
            Hero? attacker = GameData._heroes.FirstOrDefault(x => x.Id == idCaster);
            AttackService.SetDamage(attacker, defender, value, Consts.DamageType.Pure);
        }

        public override void RemoveEffect(Hero _hero)
        {

        }
    }
}
