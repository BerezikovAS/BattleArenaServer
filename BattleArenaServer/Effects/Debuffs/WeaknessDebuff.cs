using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Debuffs
{
    public class WeaknessDebuff : Effect
    {
        public WeaknessDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "Weakness";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = $"Урон от атак снижен на {value}%";
        }

        public override void ApplyEffect(Hero _hero)
        {
            _hero.StatsEffect.DmgMultiplier -= Convert.ToDouble(value) / 100;
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.StatsEffect.DmgMultiplier += Convert.ToDouble(value) / 100;
        }
    }
}
