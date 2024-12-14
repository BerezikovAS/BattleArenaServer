using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.CastCheckers
{
    public class EnemyTargetSpellChecker : ICastChecker
    {
        public ICastChecker nextChecker { get; set; } = new TerminalChecker();

        public bool Check(RequestData requestData, Skill skill)
        {
            // Если кастуем в союзника или у цели не в смоке, пропускаем дальше
            if (requestData.Caster?.Team == requestData.Target?.Team || requestData.Target?.EffectList.FirstOrDefault(x => x.Name == "Smoke") == null)
                return nextChecker.Check(requestData, skill);
            return false;
        }
    }
}
