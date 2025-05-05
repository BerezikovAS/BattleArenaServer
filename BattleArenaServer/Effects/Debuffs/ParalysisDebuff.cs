using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Debuffs
{
    public class ParalysisDebuff : Effect
    {
        public ParalysisDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "Paralysis";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = "Персонаж парализован. Перемещение, атака, использование предметов и способностей недоступны";

            effectTags.Add(Consts.EffectTag.Disarm);
            effectTags.Add(Consts.EffectTag.Silence);
            effectTags.Add(Consts.EffectTag.Root);
            effectTags.Add(Consts.EffectTag.NonItem);
        }

        public override void ApplyEffect(Hero hero)
        {

        }

        public override void RemoveEffect(Hero hero)
        {

        }
    }
}
