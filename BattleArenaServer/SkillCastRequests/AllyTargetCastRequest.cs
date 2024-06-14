using BattleArenaServer.CastCheckers;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.SkillCastRequests
{
    public class AllyTargetCastRequest : ISkillCastRequest
    {
        public bool startRequest(RequestData requestData, Skill skill)
        {
            ICastChecker coolDownChecker = new CooldownChecker();
            ICastChecker actionPointsChecker = new ActionPointsChecker();
            ICastChecker rangeChecker = new RangeChecker();
            ICastChecker allyChecker = new AllyChecker();

            coolDownChecker.nextChecker = actionPointsChecker;
            actionPointsChecker.nextChecker = rangeChecker;
            rangeChecker.nextChecker = allyChecker;
            allyChecker.nextChecker = new TerminalChecker();
            return coolDownChecker.Check(requestData, skill);
        }
    }
}
