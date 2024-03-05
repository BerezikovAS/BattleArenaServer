using BattleArenaServer.CastCheckers;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.SkillCastRequests
{
    public class RangeAoECastRequest : ISkillCastRequest
    {
        public void cancelRequest()
        {
            throw new NotImplementedException();
        }

        public bool startRequest(List<Hex> _hexes, int _target, int _caster, Skill _skill)
        {
            ICastChecker coolDownChecker = new CooldownChecker();
            ICastChecker rangeChecker = new RanageChecker();
            ICastChecker actionPointsChecker = new ActionPointsChecker();

            coolDownChecker.nextChecker = rangeChecker;
            rangeChecker.nextChecker = actionPointsChecker;
            actionPointsChecker.nextChecker = new TerminalChecker();
            return coolDownChecker.Check(_hexes, _target, _caster, _skill);
        }
    }
}
