using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.CastCheckers
{
    public class SelfChecker : ICastChecker
    {
        public ICastChecker nextChecker { get; set; } = new TerminalChecker();

        public bool Check(RequestData requestData, Skill skill)
        {
            if (requestData.Target?.Id != requestData.Caster?.Id)
                return false;
            return nextChecker.Check(requestData, skill);
        }
    }
}
