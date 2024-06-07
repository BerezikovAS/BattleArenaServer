using BattleArenaServer.Models;

namespace BattleArenaServer.Interfaces
{
    public interface ISkillCastRequest
    {
        public bool startRequest(Hero caster, Hero? target, Hex? targetHex, Skill skill);

        public void cancelRequest();
    }
}
