using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Buffs
{
    public class EngageBuff : Effect
    {
        public EngageBuff(int _idCaster, int _value, int _duration)
        {
            Name = "Engage";
            type = Consts.StatusEffect.Buff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = "Урон увеличена на " + value;
        }

        public override void ApplyEffect(Hero _hero)
        {
            _hero.Dmg += value;
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.Dmg -= value;
        }
    }
}
