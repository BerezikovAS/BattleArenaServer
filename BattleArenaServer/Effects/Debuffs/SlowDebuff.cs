using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Debuffs
{
    public class SlowDebuff : Effect
    {
        public SlowDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "Slow";
            type = "debuff";
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = "Стоимость передвижения увеличена на 1 ОД.";
        }

        public override void ApplyEffect(Hero _hero)
        {
            _hero.MoveSkill.requireAP += 1;
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.MoveSkill.requireAP -= 1;
        }
    }
}
