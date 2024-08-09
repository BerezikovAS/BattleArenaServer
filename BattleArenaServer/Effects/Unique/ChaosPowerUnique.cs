using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Unique
{
    public class ChaosPowerUnique : Effect
    {
        int[] chaosPoints = { 0, 0, 0 };
        public ChaosPowerUnique(int _idCaster, int _value, int _duration)
        {
            Name = "ChaosPower";
            type = "unique";
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            effectType = Consts.EffectType.Instant;
            description = "Броня уменьшена на " + value;
        }

        public override void ApplyEffect(Hero _hero)
        {
            Random rnd = new Random();
            chaosPoints[0] = rnd.Next(0, value + 1);
            value -= chaosPoints[0];
            chaosPoints[1] = rnd.Next(0, value + 1);
            value -= chaosPoints[1];
            chaosPoints[2] = value;

            _hero.StatsEffect.Dmg += chaosPoints[0] * 8;
            _hero.StatsEffect.Armor += chaosPoints[1];
            _hero.StatsEffect.Resist += chaosPoints[2];
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.StatsEffect.Dmg -= chaosPoints[0] * 8;
            _hero.StatsEffect.Armor -= chaosPoints[1];
            _hero.StatsEffect.Resist -= chaosPoints[2];
        }
    }
}
