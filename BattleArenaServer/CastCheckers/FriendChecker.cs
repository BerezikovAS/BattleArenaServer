using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.CastCheckers
{
    public class FriendChecker : ICastChecker
    {
        public ICastChecker nextChecker { get; set; } = new TerminalChecker();

        public bool Check(RequestData requestData, Skill skill)
        {
            if (requestData.Caster?.Team == requestData.Target?.Team && requestData.Caster?.Id != requestData.Target?.Id)
                return nextChecker.Check(requestData, skill);
            return false;
        }
    }
}
