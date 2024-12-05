using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Buffs
{
    public class PhysShieldBuff : Effect
    {
        public PhysShieldBuff(int _idCaster, int _value, int _duration)
        {
            Name = "PhysShield";
            type = Consts.StatusEffect.Buff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = $"Щит поглощает {value} физического урона";
        }

        public override void ApplyEffect(Hero _hero)
        {

        }

        public override void RemoveEffect(Hero _hero)
        {

        }

        public void RefreshDescr()
        {
            description = $"Щит поглощает {value} физического урона";
        }
    }
}
