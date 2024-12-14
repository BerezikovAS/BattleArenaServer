using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Unique
{
    public class TotemChargeUnique : Effect
    {
        public TotemChargeUnique(int _idCaster, int _value, int _duration)
        {
            Name = "TotemCharge";
            type = Consts.StatusEffect.Unique;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = $"{value} зарядов.";
        }

        public override void ApplyEffect(Hero _hero)
        {
            value++;
            description = $"{value} зарядов.";
        }

        public override void RemoveEffect(Hero _hero)
        {

        }
    }
}
