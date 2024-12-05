using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Debuffs
{
    public class DecayDebuff : Effect
    {
        public DecayDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "Decay";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = "Этот герой не может восполнять здоровье.";
        }

        public override void ApplyEffect(Hero _hero)
        {
            _hero.Heal -= _hero.BaseHeal;
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.Heal += _hero.BaseHeal;
        }
    }
}
