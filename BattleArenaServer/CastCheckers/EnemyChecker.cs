using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.CastCheckers
{
    public class EnemyChecker : ICastChecker
    {
        public ICastChecker nextChecker { get; set; } = new TerminalChecker();

        public bool Check(Hero caster, Hero? target, Hex? targetHex, Skill skill)
        {
            if (caster != null && target != null)
            {
                if (caster.Team != target.Team)
                    return nextChecker.Check(caster, target, targetHex, skill);
            }
            return false;
        }
    }
}
