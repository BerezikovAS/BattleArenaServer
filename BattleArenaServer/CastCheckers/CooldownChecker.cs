using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using System.Collections.Generic;

namespace BattleArenaServer.CastCheckers
{
    public class CooldownChecker : ICastChecker
    {
        public ICastChecker nextChecker { get; set; }

        public bool Check(List<Hex> _hexes, int _target, int _caster, Skill _skill)
        {
            if (_skill.coolDownNow > 0)
                return false;
            return nextChecker.Check(_hexes, _target, _caster, _skill);
        }
    }
}
