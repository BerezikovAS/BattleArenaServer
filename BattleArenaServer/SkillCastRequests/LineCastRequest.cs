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

        public bool startRequest(List<Hex> _hexes, int _target, int _caster, Skill _skill)
        {
            ICastChecker coolDownChecker = new CooldownChecker();
            ICastChecker actionPointsChecker = new ActionPointsChecker();
            ICastChecker onLineChecker = new OnLineChecker();

            coolDownChecker.nextChecker = actionPointsChecker;
            actionPointsChecker.nextChecker = onLineChecker;
            onLineChecker.nextChecker = new TerminalChecker();
            return coolDownChecker.Check(_hexes, _target, _caster, _skill);
        }
    }
}
