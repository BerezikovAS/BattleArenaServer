using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Debuffs
{
    public class DisarmDebuff : Effect
    {
        public DisarmDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "Disarm";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = "Персонаж не может атаковать";

            effectTags.Add(Consts.EffectTag.Disarm);
        }

        public override void ApplyEffect(Hero _hero)
        {

        }

        public override void RemoveEffect(Hero _hero)
        {

        }
    }
}
