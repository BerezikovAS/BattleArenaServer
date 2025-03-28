using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Unique
{
    public class ImmunUnique : Effect
    {
        public ImmunUnique(int _idCaster, int _value, int _duration)
        {
            Name = "Immun";
            type = Consts.StatusEffect.Unique;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = $"Персонаж невосприимчив к негативным эффектам.";
        }

        public override void ApplyEffect(Hero _hero)
        {
            _hero.AddEffect = _hero.ImmunAddEffect;
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.AddEffect = _hero.BaseAddEffect;
        }
    }
}
