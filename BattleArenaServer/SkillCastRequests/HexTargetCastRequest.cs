using BattleArenaServer.CastCheckers;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.SkillCastRequests
{
    public class HexTargetCastRequest : ISkillCastRequest
    {
        public void cancelRequest()
        {
            throw new NotImplementedException();
        }

        public bool startRequest(Hero caster, Hero? target, Hex? targetHex, Skill skill)
        {
            ICastChecker coolDownChecker = new CooldownChecker();
            ICastChecker actionPointsChecker = new ActionPointsChecker();
            ICastChecker rangeChecker = new RangeChecker();

            coolDownChecker.nextChecker = actionPointsChecker;
            actionPointsChecker.nextChecker = rangeChecker;
            rangeChecker.nextChecker = new TerminalChecker();
            return coolDownChecker.Check(caster, target, targetHex, skill);
        }
    }
}
