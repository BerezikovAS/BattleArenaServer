using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Debuffs
{
    public class SilenceDebuff : Effect
    {
        public SilenceDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "Silence";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = "Персонаж не может использовать способности";

            effectTags.Add(Consts.EffectTag.Silence);
        }

        public override void ApplyEffect(Hero _hero)
        {

        }

        public override void RemoveEffect(Hero _hero)
        {

        }
    }
}
