using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Debuffs
{
    public class CondemnationDebuff : Effect
    {
        public CondemnationDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "Condemnation";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = "Входящий урон увеличен на " + value + "%";
        }

        public override void ApplyEffect(Hero hero)
        {
            hero.modifierAppliedDamage += Condemnation;
        }

        public override void RemoveEffect(Hero hero)
        {
            hero.modifierAppliedDamage -= Condemnation;
        }

        private int Condemnation(Hero attacker, Hero defender, int dmg)
        {
            double extraDamage = dmg * value * 0.01;
            return (int)Math.Round(extraDamage);
        }
    }
}
