using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.CastCheckers
{
    public class MoveEffectChecker : ICastChecker
    {
        public ICastChecker nextChecker { get; set; } = new TerminalChecker();

        public bool Check(RequestData requestData, Skill skill)
        {
            if (requestData.Caster?.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Root)) == null)
                return nextChecker.Check(requestData, skill);
            return false;
        }
    }
}
