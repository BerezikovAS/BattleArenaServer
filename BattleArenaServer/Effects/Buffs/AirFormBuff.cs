using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Buffs
{
    public class AirFormBuff : Effect
    {
        public AirFormBuff(int _idCaster, int _value, int _duration)
        {
            Name = "AirForm";
            type = Consts.StatusEffect.Buff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = "Входящий урон уменьшается на " + value + "%";
        }

        public override void ApplyEffect(Hero _hero)
        {
            _hero.modifierAppliedDamage += ModifierAppliedDamageDelegate;
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.modifierAppliedDamage -= ModifierAppliedDamageDelegate;
        }

        private int ModifierAppliedDamageDelegate(Hero? attacker, Hero defender, int dmg, Consts.DamageType dmgType)
        {
            double extraDamage = dmg * value * -0.01;
            return (int)Math.Round(extraDamage);
        }
    }
}
