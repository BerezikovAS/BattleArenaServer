using BattleArenaServer.CastCheckers;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.SkillCastRequests
{
    public class RangeAoECastRequest : ISkillCastRequest
    {
        public bool startRequest(RequestData requestData, Skill skill, GameData gameData)
        {
            ICastChecker coolDownChecker = new CooldownChecker();
            ICastChecker rangeChecker = new RangeChecker();
            ICastChecker actionPointsChecker = new ActionPointsChecker();

            coolDownChecker.nextChecker = rangeChecker;
            rangeChecker.nextChecker = actionPointsChecker;
            actionPointsChecker.nextChecker = new TerminalChecker();
            return coolDownChecker.Check(requestData, skill, gameData);
        }
    }
}
