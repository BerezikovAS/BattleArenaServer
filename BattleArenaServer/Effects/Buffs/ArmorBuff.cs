using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Buffs
{
    public class ArmorBuff : Effect
    {
        public ArmorBuff(int _idCaster, int _value, int _duration)
        {
            Name = "IncreaseArmor";
            type = "buff";
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
        }

        public override void ApplyEffect(Hero _hero)
        {
            _hero.Armor += value;
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.Armor -= value;
        }
    }
}
