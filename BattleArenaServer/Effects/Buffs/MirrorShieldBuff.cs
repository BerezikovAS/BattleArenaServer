using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Buffs
{
    public class MirrorShieldBuff : Effect
    {
        public MirrorShieldBuff(int _idCaster, int _value, int _duration)
        {
            Name = "MirrorShield";
            type = Consts.StatusEffect.Buff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = $"Сопротивление увеличено на {value}. Негативные эффекты отражаются обратно в заклинателя";
            effectTags.Add(Consts.EffectTag.MirrorShield);
        }

        public override void ApplyEffect(Hero _hero, GameData gameData)
        {
            _hero.StatsEffect.Resist += value;
        }

        public override void RemoveEffect(Hero _hero, GameData gameData)
        {
            _hero.StatsEffect.Resist -= value;
        }
    }
}
