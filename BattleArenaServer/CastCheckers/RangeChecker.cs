using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using System.Collections.Generic;

namespace BattleArenaServer.CastCheckers
{
    public class RangeChecker : ICastChecker
    {
        public ICastChecker nextChecker { get; set; } = new TerminalChecker();

        public bool Check(Hero caster, Hero? target, Hex? targetHex, Skill skill)
        {
            Hex? casterHex = GameData._hexes.FirstOrDefault(x => x.HERO?.Id == caster.Id);
            if (casterHex != null && targetHex != null && skill.range < casterHex.Distance(targetHex))
                return false;
            return nextChecker.Check(caster, target, targetHex, skill);
        }
    }
}
