using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Debuffs
{
    public class ClimaxDebuff : Effect
    {
        public ClimaxDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "Climax";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = $"Следующий полученный урон целью увеличится на {value}%";
        }

        public override void ApplyEffect(Hero hero, GameData gameData)
        {
            hero.modifierAppliedDamage += Climax;
        }

        public override void RemoveEffect(Hero hero, GameData? gameData)
        {
            hero.modifierAppliedDamage -= Climax;
        }

        private int Climax(Hero? attacker, Hero defender, int dmg, Consts.DamageType dmgType)
        {
            this.RemoveEffect(defender, null);
            defender.EffectList.Remove(this);

            double extraDamage = dmg * value * 0.01;
            return (int)Math.Round(extraDamage);
        }
    }
}
