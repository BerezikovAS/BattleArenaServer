using BattleArenaServer.CastCheckers;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.SkillCastRequests
{
    public class AllyTargetCastRequest : ISkillCastRequest
    {
        public void cancelRequest()
        {
            throw new NotImplementedException();
        }

        public bool startRequest(List<Hex> _hexes, int _target, int _caster, Skill _skill)
        {
            ICastChecker coolDownChecker = new CooldownChecker();
            ICastChecker actionPointsChecker = new ActionPointsChecker();
            ICastChecker rangeChecker = new RanageChecker();
            ICastChecker allyChecker = new AllyChecker();

            coolDownChecker.nextChecker = actionPointsChecker;
            actionPointsChecker.nextChecker = rangeChecker;
            rangeChecker.nextChecker = allyChecker;
            allyChecker.nextChecker = new TerminalChecker();
            return coolDownChecker.Check(_hexes, _target, _caster, _skill);
        }
    }
}
