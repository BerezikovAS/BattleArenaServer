using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Debuffs
{
    public class BurnDebuff : Effect
    {
        public BurnDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "Burn";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = $"Каждая потеря здоровья отнимает еще {value}% от максимального запаса";
        }

        public override void ApplyEffect(Hero hero)
        {
            hero.modifierAppliedDamage += Burn;
        }

        public override void RemoveEffect(Hero hero)
        {
            hero.modifierAppliedDamage -= Burn;
        }

        private int Burn(Hero? attacker, Hero defender, int dmg, Consts.DamageType dmgType)
        {
            double extraDamage = defender.MaxHP * value * 0.01;
            return (int)Math.Round(extraDamage);
        }
    }
}
