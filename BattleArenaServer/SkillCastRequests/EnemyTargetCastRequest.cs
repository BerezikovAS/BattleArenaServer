using BattleArenaServer.CastCheckers;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.SkillCastRequests
{
    public class EnemyTargetCastRequest : ISkillCastRequest
    {
        public bool startRequest(RequestData requestData, Skill skill)
        {
            ICastChecker coolDownChecker = new CooldownChecker();
            ICastChecker actionPointsChecker = new ActionPointsChecker();
            ICastChecker rangeChecker = new RangeChecker();
            ICastChecker enemyChecker = new EnemyChecker();
            ICastChecker enemyTargetSpellChecker = new EnemyTargetSpellChecker();

            coolDownChecker.nextChecker = actionPointsChecker;
            actionPointsChecker.nextChecker = rangeChecker;
            rangeChecker.nextChecker = enemyChecker;
            enemyChecker.nextChecker = enemyTargetSpellChecker;
            enemyTargetSpellChecker.nextChecker = new TerminalChecker();
            return coolDownChecker.Check(requestData, skill);
        }
    }
}
