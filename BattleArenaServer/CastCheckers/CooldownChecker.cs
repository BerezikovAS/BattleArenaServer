using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using System.Collections.Generic;

namespace BattleArenaServer.CastCheckers
{
    public class CooldownChecker : ICastChecker
    {
        public ICastChecker nextChecker { get; set; } = new TerminalChecker();

        public bool Check(RequestData requestData, Skill skill)
        {
            if (skill.coolDownNow > 0)
                return false;
            return nextChecker.Check(requestData, skill);
        }
    }
}
