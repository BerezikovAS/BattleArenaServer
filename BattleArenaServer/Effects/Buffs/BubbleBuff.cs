using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Buffs
{
    public class BubbleBuff : Effect
    {
        public BubbleBuff(int _idCaster, int _value, int _duration)
        {
            Name = "Bubble";
            type = Consts.StatusEffect.Buff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = "Игнорируйте первое получение урона.";

            effectTags.Add(Consts.EffectTag.Bubble);
        }

        public override void ApplyEffect(Hero _hero)
        {

        }

        public override void RemoveEffect(Hero _hero)
        {

        }
    }
}
