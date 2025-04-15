using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Debuffs
{
    public class SlowDebuff : Effect
    {
        public SlowDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "Slow";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = "Стоимость передвижения увеличена на 1 ОД.";
            effectTags.Add(Consts.EffectTag.Slow);
        }

        public override void ApplyEffect(Hero _hero)
        {

        }

        public override void RemoveEffect(Hero _hero)
        {

        }
    }
}
