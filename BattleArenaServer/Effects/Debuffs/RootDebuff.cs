using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Debuffs
{
    public class RootDebuff : Effect
    {
        public RootDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "Root";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = "Персонаж не может перемещаться";
        }

        public override void ApplyEffect(Hero _hero)
        {

        }

        public override void RemoveEffect(Hero _hero)
        {

        }
    }
}
