using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.CastCheckers
{
    public class RangeChecker : ICastChecker
    {
        public ICastChecker nextChecker { get; set; } = new TerminalChecker();

        public bool Check(RequestData requestData, Skill skill)
        {
            int range = requestData.Caster?.EffectList.FirstOrDefault(x => x.Name == "Blind") == null ? skill.range : 1;

            if (requestData.CasterHex != null && requestData.TargetHex != null && range < requestData.CasterHex.Distance(requestData.TargetHex))
                return false;
            return nextChecker.Check(requestData, skill);
        }
    }
}
