using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.CastCheckers
{
    public class OnLineChecker : ICastChecker
    {
        public ICastChecker nextChecker { get; set; } = new TerminalChecker();

        public bool Check(Hero caster, Hero? target, Hex? targetHex, Skill skill)
        {
            Hex? casterHex = GameData._hexes.FirstOrDefault(x => x.HERO?.Id == caster.Id);
            if (targetHex != null && casterHex != null)
            {
                if (UtilityService.IsOnLine(casterHex, targetHex))
                    return nextChecker.Check(caster, target, targetHex, skill);
            }
            return false;
        }
    }
}
