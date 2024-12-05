using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Buffs
{
    public class RitualUnique : Effect
    {
        public RitualUnique(int _idCaster, int _value, int _duration)
        {
            Name = "Ritual";
            type = Consts.StatusEffect.Unique;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = $"{value} очков ритуала.";
        }

        public override void ApplyEffect(Hero _hero)
        {
            value++;
            description = $"{value} очков ритуала.";
        }

        public override void RemoveEffect(Hero _hero)
        {

        }
    }
}
