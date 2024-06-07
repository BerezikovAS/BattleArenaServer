using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.CastCheckers
{
    public class SelfChecker : ICastChecker
    {
        public ICastChecker nextChecker { get; set; } = new TerminalChecker();

        public bool Check(Hero caster, Hero? target, Hex? targetHex, Skill skill)
        {
            if (target != null && target.Id != caster.Id)
                return false;
            return nextChecker.Check(caster, target, targetHex, skill);
        }
    }
}
