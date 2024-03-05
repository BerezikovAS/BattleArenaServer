using BattleArenaServer.Models;

namespace BattleArenaServer.Interfaces
{
    public interface ICastChecker
    {
        ICastChecker nextChecker { get; set; }

        bool Check(List<Hex> _hexes, int _target, int _caster, Skill _skill);
    }
}
