using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.CastCheckers
{
    public class SelfChecker : ICastChecker
    {
        public ICastChecker nextChecker { get; set; }

        public bool Check(List<Hex> _hexes, int _target, int _caster, Skill _skill)
        {
            if (_target != _caster)
                return false;
            return nextChecker.Check(_hexes, _target, _caster, _skill);
        }
    }
}
