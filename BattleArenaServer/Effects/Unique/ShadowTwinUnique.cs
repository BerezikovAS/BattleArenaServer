using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Unique
{
    public class ShadowTwinUnique : Effect
    {
        public ShadowTwinUnique(int _idCaster, int _value, int _duration)
        {
            Name = "ShadowTwin";
            type = Consts.StatusEffect.Unique;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = $"Теневая копия наносит 50% урона и получает на 400% урона больше.";
        }

        public override void ApplyEffect(Hero _hero)
        {
            _hero.passiveAttackDamage += ReduceAttackDmg;
            _hero.modifierAppliedDamage += IncreaseDealedDmg;
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.passiveAttackDamage -= ReduceAttackDmg;
            _hero.modifierAppliedDamage -= IncreaseDealedDmg;
        }

        private int ReduceAttackDmg(Hero attacker, Hero? defender)
        {
            return (int)(Convert.ToDouble(attacker.Dmg) * -0.5); //Отнимаем 50% от урона, чтобы получить 50%
        }

        private int IncreaseDealedDmg(Hero attacker, Hero? defender, int dmg, Consts.DamageType dmgType)
        {
            return dmg * 4; //Увеличиваем получаемый урон в 5 раз т.е. на 400%
        }
    }
}
