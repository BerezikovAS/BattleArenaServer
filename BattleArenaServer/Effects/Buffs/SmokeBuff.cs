using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Buffs
{
    public class SmokeBuff : Effect
    {
        public SmokeBuff(int _idCaster, int _value, int _duration)
        {
            Name = "Smoke";
            type = Consts.StatusEffect.Buff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = $"Броня увеличена на {value}. Невозможно выбрать целью атаки или заклинания.";

            effectTags.Add(Consts.EffectTag.NonTargetable);
        }

        public override void ApplyEffect(Hero _hero)
        {
            _hero.StatsEffect.Armor += value;
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.StatsEffect.Armor -= value;
        }
    }
}
