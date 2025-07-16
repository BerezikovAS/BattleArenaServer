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
            description = $"Теневая копия наносит 60% урона и получает на 320% урона больше.";
        }

        public override void ApplyEffect(Hero _hero, GameData gameData)
        {
            _hero.passiveAttackDamage += ReduceAttackDmg;
            _hero.modifierAppliedDamage += IncreaseDealedDmg;
        }

        public override void RemoveEffect(Hero _hero, GameData gameData)
        {
            _hero.passiveAttackDamage -= ReduceAttackDmg;
            _hero.modifierAppliedDamage -= IncreaseDealedDmg;
        }

        private int ReduceAttackDmg(Hero attacker, Hero? defender, GameData gameData)
        {
            return (int)(Convert.ToDouble(attacker.Dmg) * -0.4); //Отнимаем 40% от урона, чтобы получить 60%
        }

        private int IncreaseDealedDmg(Hero attacker, Hero? defender, int dmg, Consts.DamageType dmgType)
        {
            return (int)(Convert.ToDouble(dmg) * 3.2); //Увеличиваем получаемый урон на 320%
        }
    }
}
