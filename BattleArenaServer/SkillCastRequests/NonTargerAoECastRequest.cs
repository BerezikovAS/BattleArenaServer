using BattleArenaServer.CastCheckers;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.SkillCastRequests
{
    public class NonTargerAoECastRequest : ISkillCastRequest
    {
        public bool startRequest(RequestData requestData, Skill skill)
        {
            ICastChecker coolDownChecker = new CooldownChecker();
            ICastChecker selfChecker = new SelfChecker();
            ICastChecker actionPointsChecker = new ActionPointsChecker();

            coolDownChecker.nextChecker = selfChecker;
            selfChecker.nextChecker = actionPointsChecker;
            actionPointsChecker.nextChecker = new TerminalChecker();
            return coolDownChecker.Check(requestData, skill);
        }
    }
}
