using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.CastCheckers
{
    public class HexChecker : ICastChecker
    {
        public ICastChecker nextChecker { get; set; } = new TerminalChecker();

        public bool Check(RequestData requestData, Skill skill)
        {
            if (requestData.TargetHex != null && requestData.TargetHex.IsFree())
                return nextChecker.Check(requestData, skill);
            return false;
        }
    }
}
