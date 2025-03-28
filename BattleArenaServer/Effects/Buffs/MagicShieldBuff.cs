using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Buffs
{
    public class MagicShieldBuff : Effect
    {
        public MagicShieldBuff(int _idCaster, int _value, int _duration)
        {
            Name = "MagicShield";
            type = Consts.StatusEffect.Buff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = $"Щит поглощает {value} магического урона";

            effectTags.Add(Consts.EffectTag.MagicShield);
        }

        public override void ApplyEffect(Hero _hero)
        {

        }

        public override void RemoveEffect(Hero _hero)
        {

        }

        public override void RefreshDescr()
        {
            description = $"Щит поглощает {value} магического урона";
        }
    }
}
