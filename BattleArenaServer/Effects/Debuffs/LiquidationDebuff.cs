using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Debuffs
{
    public class LiquidationDebuff : Effect
    {
        public LiquidationDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "Liquidation";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
        }

        public override void ApplyEffect(Hero hero)
        {
            hero.modifierAppliedDamage += Liquidation;
            description = $"Атаки Assassin дополнительно отнимают {value} ХП.";
        }

        public override void RemoveEffect(Hero hero)
        {
            hero.modifierAppliedDamage -= Liquidation;
        }

        private int Liquidation(Hero attacker, Hero defender, int dmg)
        {
            if (attacker.Id == idCaster)
                return value;
            return 0;
        }
    }
}
