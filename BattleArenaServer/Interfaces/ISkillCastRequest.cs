using BattleArenaServer.Models;

namespace BattleArenaServer.Interfaces
{
    public interface ISkillCastRequest
    {
        public bool startRequest(List<Hex> _hexes, int _target, int _caster, Skill _skill);

        public void cancelRequest();
    }
}
