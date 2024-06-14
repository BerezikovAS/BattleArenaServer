using BattleArenaServer.CastCheckers;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.SkillCastRequests
{
    public class NontargetCastRequest : ISkillCastRequest
    {
        public bool startRequest(RequestData requestData, Skill skill)
        {
            ICastChecker coolDownChecker = new CooldownChecker();
            ICastChecker actionPointsChecker = new ActionPointsChecker();
            coolDownChecker.nextChecker = actionPointsChecker;
            actionPointsChecker.nextChecker = new TerminalChecker();
            return coolDownChecker.Check(requestData, skill);
        }
    }
}
