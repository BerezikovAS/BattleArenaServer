using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Unique
{
    public class ChaosPowerUnique : Effect
    {
        int[] chaosPoints = { 0, 0, 0 };
        bool removed = false;
        public ChaosPowerUnique(int _idCaster, int _value, int _duration)
        {
            Name = "ChaosPower";
            type = Consts.StatusEffect.Unique;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            effectType = Consts.EffectType.Instant;
        }

        public override void ApplyEffect(Hero _hero)
        {
            removed = false;
            Random rnd = new Random();
            chaosPoints[0] = rnd.Next(0, value + 1);
            value -= chaosPoints[0];
            chaosPoints[1] = rnd.Next(0, value + 1);
            value -= chaosPoints[1];
            chaosPoints[2] = value;

            _hero.Dmg += chaosPoints[0] * 8;
            _hero.Armor += chaosPoints[1];
            _hero.Resist += chaosPoints[2];

            description = "";
            description += chaosPoints[0] > 0 ? $"Урон увеличен на {chaosPoints[0] * 8}\n" : "";
            description += chaosPoints[1] > 0 ? $"Броня увеличена на {chaosPoints[1]}\n" : "";
            description += chaosPoints[2] > 0 ? $"Сопротивление увеличено на {chaosPoints[2]}" : "";
        }

        public override void RemoveEffect(Hero _hero)
        {
            if (!removed)
            {
                _hero.Dmg -= chaosPoints[0] * 8;
                _hero.Armor -= chaosPoints[1];
                _hero.Resist -= chaosPoints[2];
                removed = true;
            }
        }
    }
}
