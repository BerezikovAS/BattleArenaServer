using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Buffs
{
    public class EagleEyeBuff : Effect
    {
        public EagleEyeBuff(int _idCaster, int _value, int _duration)
        {
            Name = "EagleEye";
            type = "buff";
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
        }

        public override void ApplyEffect(Hero _hero)
        {
            _hero.StatsEffect.AttackRadius += 1;
            _hero.StatsEffect.Dmg += value;
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.StatsEffect.AttackRadius -= 1;
            _hero.StatsEffect.Dmg -= value;
        }
    }
}
