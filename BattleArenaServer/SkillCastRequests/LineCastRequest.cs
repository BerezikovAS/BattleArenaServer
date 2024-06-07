using BattleArenaServer.CastCheckers;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.SkillCastRequests
{
    public class LineCastRequest : ISkillCastRequest
    {
        public void cancelRequest()
        {
            throw new NotImplementedException();
        }

        public bool startRequest(Hero caster, Hero? target, Hex? targetHex, Skill skill)
        {
            ICastChecker coolDownChecker = new CooldownChecker();
            ICastChecker actionPointsChecker = new ActionPointsChecker();
            ICastChecker onLineChecker = new OnLineChecker();

            coolDownChecker.nextChecker = actionPointsChecker;
            actionPointsChecker.nextChecker = onLineChecker;
            onLineChecker.nextChecker = new TerminalChecker();
            return coolDownChecker.Check(caster, target, targetHex, skill);
        }
    }
}
