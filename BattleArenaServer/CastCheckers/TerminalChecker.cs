using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.CastCheckers
{
    public class TerminalChecker : ICastChecker
    {
        public ICastChecker nextChecker { get; set; }

        public bool Check(Hero caster, Hero? target, Hex? targetHex, Skill skill)
        {
            return true;
        }
    }
}
