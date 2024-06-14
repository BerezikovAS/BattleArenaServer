using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.CastCheckers
{
    public class ActionPointsChecker : ICastChecker
    {
        public ICastChecker nextChecker { get; set; } = new TerminalChecker();

        public bool Check(RequestData requestData, Skill skill)
        {
            if(requestData.Caster?.AP >= skill.requireAP)
                return nextChecker.Check(requestData, skill);
            return false;
        }
    }
}
