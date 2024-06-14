using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.CastCheckers
{
    public class OnLineChecker : ICastChecker
    {
        public ICastChecker nextChecker { get; set; } = new TerminalChecker();

        public bool Check(RequestData requestData, Skill skill)
        {
            if (requestData.TargetHex != null && requestData.CasterHex != null)
            {
                if (UtilityService.IsOnLine(requestData.CasterHex, requestData.TargetHex))
                    return nextChecker.Check(requestData, skill);
            }
            return false;
        }
    }
}
