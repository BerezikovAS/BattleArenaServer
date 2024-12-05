using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Unique
{
    public class AdrenalinUnique : Effect
    {
        public AdrenalinUnique(int _idCaster, int _value, int _duration)
        {
            Name = "Adrenalin";
            type = Consts.StatusEffect.Unique;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = $"+{value} очков действия.";
        }

        public override void ApplyEffect(Hero _hero)
        {
            _hero.AP += value;
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.AP -= value;
        }
    }
}
