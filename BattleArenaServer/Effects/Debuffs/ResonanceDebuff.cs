using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Debuffs
{
    public class ResonanceDebuff : Effect
    {
        public ResonanceDebuff(int _idCaster, int _value, int _duration, string _casterName)
        {
            Name = "Resonance";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = $"Урон от атак по {_casterName} снижен на {value}%";
        }

        public override void ApplyEffect(Hero _hero)
        {
            _hero.passiveAttackDamage += PassiveAttackDamage;
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.passiveAttackDamage -= PassiveAttackDamage;
        }


        private int PassiveAttackDamage(Hero attacker, Hero? defender)
        {
            if (defender != null && defender.Id == idCaster)
                return (int)(Convert.ToDouble(attacker.Dmg) * value / 100 * -1);
            return 0;
        }
    }
}
