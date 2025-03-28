using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Buffs
{
    public class HasteBuff : Effect
    {
        public HasteBuff(int _idCaster, int _value, int _duration)
        {
            Name = "Haste";
            type = Consts.StatusEffect.Buff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = "Вы можете переместиться, не тратя очки действий.";

            effectTags.Add(Consts.EffectTag.Haste);
        }

        public override void ApplyEffect(Hero _hero)
        {

        }

        public override void RemoveEffect(Hero _hero)
        {

        }
    }
}
