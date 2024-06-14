using BattleArenaServer.Models;

namespace BattleArenaServer.Interfaces
{
    public interface ICastChecker
    {
        ICastChecker nextChecker { get; set; }

        bool Check(RequestData requestData, Skill skill);
    }
}
