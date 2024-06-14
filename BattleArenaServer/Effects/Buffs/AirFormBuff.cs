using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Buffs
{
    public class AirFormBuff : Effect
    {
        // Тот кого забафали и ему срезаем входящий дамаг
        public AirFormBuff(int _idCaster, int _value, int _duration)
        {
            Name = "AirForm";
            type = "buff";
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
        }

        public override void ApplyEffect(Hero _hero)
        {
            _hero.modifierAppliedDamage += ModifierAppliedDamageDelegate;
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.modifierAppliedDamage -= ModifierAppliedDamageDelegate;
        }

        private int ModifierAppliedDamageDelegate(Hero? attacker, Hero defender, int dmg)
        {
            double extraDamage = dmg * value * -0.01;
            return (int)Math.Round(extraDamage);
        }
    }
}
