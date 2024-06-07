using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Debuffs
{
    public class ArmorDebuff : Effect
    {
        public ArmorDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "RemoveArmor";
            type = "debuff";
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
        }

        public override void ApplyEffect(Hero _hero)
        {
            _hero.Armor -= value;
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.Armor += value;
        }
    }
}
