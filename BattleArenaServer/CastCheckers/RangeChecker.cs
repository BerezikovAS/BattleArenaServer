using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using System.Collections.Generic;

namespace BattleArenaServer.CastCheckers
{
    public class RangeChecker : ICastChecker
    {
        public ICastChecker nextChecker { get; set; } = new TerminalChecker();

        public bool Check(RequestData requestData, Skill skill)
        {
            if (requestData.CasterHex != null && requestData.TargetHex != null && skill.range < requestData.CasterHex.Distance(requestData.TargetHex))
                return false;
            return nextChecker.Check(requestData, skill);
        }
    }
}
