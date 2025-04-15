using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Debuffs
{
    public class ResistDebuff : Effect
    {
        public ResistDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "RemoveResist";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = "Сопротивление уменьшено на " + value;
        }

        public override void ApplyEffect(Hero _hero)
        {
            _hero.Resist -= value;
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.Resist += value;
        }
    }
}
