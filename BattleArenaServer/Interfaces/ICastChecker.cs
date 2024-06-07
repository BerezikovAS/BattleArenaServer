using BattleArenaServer.Models;

namespace BattleArenaServer.Interfaces
{
    public interface ICastChecker
    {
        ICastChecker nextChecker { get; set; }

        bool Check(Hero caster, Hero? target, Hex? targetHex, Skill skill);
    }
}
