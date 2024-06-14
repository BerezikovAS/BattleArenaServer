using BattleArenaServer.Models;

namespace BattleArenaServer.Interfaces
{
    public interface ISkillCastRequest
    {
        public bool startRequest(RequestData requestData, Skill skill);
    }
}
