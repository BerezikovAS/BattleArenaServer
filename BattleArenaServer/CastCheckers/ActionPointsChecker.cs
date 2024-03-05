using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.CastCheckers
{
    public class ActionPointsChecker : ICastChecker
    {
        public ICastChecker nextChecker { get; set; }

        public bool Check(List<Hex> _hexes, int _target, int _caster, Skill _skill)
        {
            Hero hero = _hexes[_caster].HERO;
            if(hero != null)
            {
                if(hero.AP >= _skill.requireAP)
                    return nextChecker.Check(_hexes, _target, _caster, _skill);
            }
            return false;
        }
    }
}
