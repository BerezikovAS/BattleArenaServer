using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Buffs
{
    public class GuardianAngelBuff : Effect
    {
        public GuardianAngelBuff(int _idCaster, int _value, int _duration)
        {
            Name = "GuardianAngel";
            type = Consts.StatusEffect.Buff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = "Вы невосприимчивы к физическому урону.";

            effectTags.Add(Consts.EffectTag.GuardianAngel);
        }

        public override void ApplyEffect(Hero _hero)
        {

        }

        public override void RemoveEffect(Hero _hero)
        {

        }
    }
}
