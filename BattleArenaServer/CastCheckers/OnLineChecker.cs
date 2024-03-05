using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.CastCheckers
{
    public class OnLineChecker : ICastChecker
    {
        public ICastChecker nextChecker { get; set; }

        public bool Check(List<Hex> _hexes, int _target, int _caster, Skill _skill)
        {
            UtilityService util = new UtilityService();
            Hex? target = _hexes.FirstOrDefault(x => x.ID == _target);
            Hex? caster = _hexes.FirstOrDefault(x => x.ID == _caster);
            if (target != null && caster != null)
            {
                if (util.IsOnLine(caster, target))
                    return nextChecker.Check(_hexes, _target, _caster, _skill);
            }
            return false;
        }
    }
}
