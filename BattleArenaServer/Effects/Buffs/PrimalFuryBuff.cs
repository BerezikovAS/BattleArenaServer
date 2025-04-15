using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Buffs
{
    public class PrimalFuryBuff : Effect
    {
        public PrimalFuryBuff(int _idCaster, int _value, int _duration)
        {
            Name = "PrimalFury";
            type = Consts.StatusEffect.Buff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = $"Следующая атака нанесёт +{value} урона.";
        }

        public override void ApplyEffect(Hero _hero)
        {
            _hero.passiveAttackDamage += BeforeAttackDelegate;
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.passiveAttackDamage -= BeforeAttackDelegate;
        }

        private int BeforeAttackDelegate(Hero attacker, Hero? defender)
        {
            this.RemoveEffect(attacker);
            attacker.EffectList.Remove(this);
            return value;
        }
    }
}
