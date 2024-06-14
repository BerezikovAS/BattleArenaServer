using BattleArenaServer.CastCheckers;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.SkillCastRequests
{
    public class HeroTargetCastRequest : ISkillCastRequest
    {
        public bool startRequest(RequestData requestData, Skill skill)
        {
            ICastChecker coolDownChecker = new CooldownChecker();
            ICastChecker actionPointsChecker = new ActionPointsChecker();
            ICastChecker rangeChecker = new RangeChecker();
            ICastChecker heroChecker = new HeroChecker();

            coolDownChecker.nextChecker = actionPointsChecker;
            actionPointsChecker.nextChecker = rangeChecker;
            rangeChecker.nextChecker = heroChecker;
            heroChecker.nextChecker = new TerminalChecker();
            return coolDownChecker.Check(requestData, skill);
        }
    }
}
