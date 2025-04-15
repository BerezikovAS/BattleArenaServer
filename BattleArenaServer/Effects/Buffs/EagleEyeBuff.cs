using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Buffs
{
    public class EagleEyeBuff : Effect
    {
        public EagleEyeBuff(int _idCaster, int _value, int _duration)
        {
            Name = "EagleEye";
            type = Consts.StatusEffect.Buff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = "Дальность атаки увеличена на 1.\nУрон увеличен на " + value;
        }

        public override void ApplyEffect(Hero _hero)
        {
            _hero.StatsEffect.AttackRadius += 1;
            _hero.Dmg += value;
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.StatsEffect.AttackRadius -= 1;
            _hero.Dmg -= value;
        }
    }
}
