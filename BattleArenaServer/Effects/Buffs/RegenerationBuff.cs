using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Buffs
{
    public class RegenerationBuff : Effect
    {
        public RegenerationBuff(int _idCaster, int _value, int _duration)
        {
            Name = "Regeneration";
            type = Consts.StatusEffect.Buff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            effectType = Consts.EffectType.EndTurn;
            description = "Восстанавливает " + value + "ХП в конце своего хода.";
        }

        public override void ApplyEffect(Hero hero)
        {
            hero.Heal(value);
        }

        public override void RemoveEffect(Hero hero)
        {

        }
    }
}
