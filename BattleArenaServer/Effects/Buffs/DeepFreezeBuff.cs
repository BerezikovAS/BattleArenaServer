using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Buffs
{
    public class DeepFreezeBuff : Effect
    {
        bool isApplied = false;
        public DeepFreezeBuff(int _idCaster, int _value, int _duration)
        {
            Name = "DeepFreeze";
            type = Consts.StatusEffect.Buff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = "Герой неуязвим к любому типу урона";
            effectType = Consts.EffectType.StartTurn;
            durationType = Consts.EffectDurationType.StartTurn;
            effectTags.Add(Consts.EffectTag.Disarm);
            effectTags.Add(Consts.EffectTag.Silence);
            effectTags.Add(Consts.EffectTag.Root);
            effectTags.Add(Consts.EffectTag.NonItem);
        }

        public override void ApplyEffect(Hero _hero)
        {
            if (!isApplied)
                _hero.modifierAppliedDamage += ModifierAppliedDamageDelegate;
            isApplied = true;
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.modifierAppliedDamage -= ModifierAppliedDamageDelegate;
        }

        private int ModifierAppliedDamageDelegate(Hero? attacker, Hero defender, int dmg, Consts.DamageType dmgType)
        {
            return dmg * -1;
        }
    }
}
