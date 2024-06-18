using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.CastCheckers
{
    public class HeroNotSelfChecker : ICastChecker
    {
        public ICastChecker nextChecker { get; set; } = new TerminalChecker();

        public bool Check(RequestData requestData, Skill skill)
        {
            if (requestData.Target?.Id != requestData.Caster?.Id && requestData.Target?.type != Consts.HeroType.Obstacle)
                return nextChecker.Check(requestData, skill);
            return false;
        }
    }
}
