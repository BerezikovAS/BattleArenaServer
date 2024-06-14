using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.CastCheckers
{
    public class TerminalChecker : ICastChecker
    {
        public ICastChecker nextChecker { get; set; }

        public bool Check(RequestData requestData, Skill skill)
        {
            return true;
        }
    }
}
