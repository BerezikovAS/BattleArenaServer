using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Buffs
{
    public class ResistBuff : Effect
    {
        public ResistBuff(int _idCaster, int _value, int _duration)
        {
            Name = "AddResist";
            type = Consts.StatusEffect.Buff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = "Сопротивление увеличено на " + value;
        }

        public override void ApplyEffect(Hero _hero)
        {
            _hero.StatsEffect.Resist += value;
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.StatsEffect.Resist -= value;
        }
    }
}
