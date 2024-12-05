using BattleArenaServer.CastCheckers;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.SkillCastRequests
{
    public class HeroNotSelfCastRequest : ISkillCastRequest
    {
        public bool startRequest(RequestData requestData, Skill skill)
        {
            ICastChecker coolDownChecker = new CooldownChecker();
            ICastChecker actionPointsChecker = new ActionPointsChecker();
            ICastChecker rangeChecker = new RangeChecker();
            ICastChecker heroNotSelfChecker = new HeroNotSelfChecker();
            ICastChecker enemyTargetSpellChecker = new EnemyTargetSpellChecker();

            coolDownChecker.nextChecker = actionPointsChecker;
            actionPointsChecker.nextChecker = rangeChecker;
            rangeChecker.nextChecker = heroNotSelfChecker;
            heroNotSelfChecker.nextChecker = enemyTargetSpellChecker;
            enemyTargetSpellChecker.nextChecker = new TerminalChecker();
            return coolDownChecker.Check(requestData, skill);
        }
    }
}
