using BattleArenaServer.Effects;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.CastCheckers
{
    public class ActionPointsChecker : ICastChecker
    {
        public ICastChecker nextChecker { get; set; } = new TerminalChecker();

        public bool Check(RequestData requestData, Skill skill)
        {
            if (requestData.Caster == null)
                return false;

            int availableAP = requestData.Caster.AP;

            // Спиритическая связь объединяет ОД двух героев, поэтому доступные ОД - это ОД кастера + ОД связанного героя
            Effect? spiritLink = requestData.Caster.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.SpiritLink));
            if (spiritLink != null)
            {
                Hero? anotherHero = GameData._heroes.FirstOrDefault(x => x.Id == spiritLink.idCaster);
                if (anotherHero != null)
                    availableAP += anotherHero.AP;
            }

            if (availableAP >= skill.requireAP)
                return nextChecker.Check(requestData, skill);
            return false;
        }
    }
}
