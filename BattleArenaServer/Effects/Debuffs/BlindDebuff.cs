using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Debuffs
{
    public class BlindDebuff : Effect
    {
        public BlindDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "Blind";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = "Дальность атаки и применения способностей снижена до 1.";
        }

        public override void ApplyEffect(Hero hero)
        {

        }

        public override void RemoveEffect(Hero hero)
        {

        }
    }
}
