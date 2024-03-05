using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.CastCheckers
{
    public class EnemyChecker : ICastChecker
    {
        public ICastChecker nextChecker { get; set; }

        public bool Check(List<Hex> _hexes, int _target, int _caster, Skill _skill)
        {
            Hero caster = _hexes[_caster].HERO;
            Hero target = _hexes[_target].HERO;
            if (caster != null & target != null)
            {
                if (caster.Team != target.Team)
                    return nextChecker.Check(_hexes, _target, _caster, _skill);
            }
            return false;
        }
    }
}
