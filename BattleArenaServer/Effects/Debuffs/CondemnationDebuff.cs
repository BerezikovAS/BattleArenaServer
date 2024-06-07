using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Effects.Debuffs
{
    public class CondemnationDebuff : Effect
    {
        public CondemnationDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "Condemnation";
            type = "debuff";
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
        }

        public override void ApplyEffect(Hero _hero)
        {

        }

        public override void RemoveEffect(Hero _hero)
        {

        }
    }
}
