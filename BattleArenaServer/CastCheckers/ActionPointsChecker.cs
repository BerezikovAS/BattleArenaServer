using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.CastCheckers
{
    public class ActionPointsChecker : ICastChecker
    {
        public ICastChecker nextChecker { get; set; } = new TerminalChecker();

        public bool Check(Hero caster, Hero? target, Hex? targetHex, Skill skill)
        {
            if(caster != null)
            {
                if(caster.AP >= skill.requireAP)
                    return nextChecker.Check(caster, target, targetHex, skill);
            }
            return false;
        }
    }
}
