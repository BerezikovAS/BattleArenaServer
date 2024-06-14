using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.CastCheckers
{
    public class HeroChecker : ICastChecker
    {
        public ICastChecker nextChecker { get; set; } = new TerminalChecker();

        public bool Check(RequestData requestData, Skill skill)
        {
            if (requestData.Target != null)
                return nextChecker.Check(requestData, skill);
            return false;
        }
    }
}
