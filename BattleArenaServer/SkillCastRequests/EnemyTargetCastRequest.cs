using BattleArenaServer.CastCheckers;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.SkillCastRequests
{
    public class EnemyTargetCastRequest : ISkillCastRequest
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
            ICastChecker enemyChecker = new EnemyChecker();

            coolDownChecker.nextChecker = actionPointsChecker;
            actionPointsChecker.nextChecker = rangeChecker;
            rangeChecker.nextChecker = enemyChecker;
            enemyChecker.nextChecker = new TerminalChecker();
            return coolDownChecker.Check(caster, target, targetHex, skill);
        }
    }
}
