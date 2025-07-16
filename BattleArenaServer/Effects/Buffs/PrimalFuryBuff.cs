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

        public override void ApplyEffect(Hero _hero, GameData gameData)
        {
            _hero.passiveAttackDamage += PassiveAttackDamage;
        }

        public override void RemoveEffect(Hero _hero, GameData gameData)
        {
            _hero.passiveAttackDamage -= PassiveAttackDamage;
        }

        private int PassiveAttackDamage(Hero attacker, Hero? defender, GameData gameData)
        {
            this.RemoveEffect(attacker, gameData);
            attacker.EffectList.Remove(this);
            return value;
        }
    }
}
