using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Skills.GeomantSkills.Auras
{
    public class StoneStrengthAura : Aura
    {
        int stalaktiteCount = 0;
        public StoneStrengthAura()
        {
            Name = "StoneStrengthAura";
            radius = 3;
            type = Consts.AuraType.Continuous;
        }

        public override void SetEffect(Hero heroSource, Hex hexSource)
        {
            stalaktiteCount = 0;
            foreach (var n in UtilityService.GetHexesRadius(hexSource, radius))
            {
                if (n.HERO != null && n.HERO.Name == "Stalaktite")
                    stalaktiteCount++;
            }
            ApplyEffect(heroSource, heroSource);
        }

        public override void ApplyEffect(Hero source, Hero target)
        {
            source.StatsEffect.Armor += stalaktiteCount;
            source.StatsEffect.Resist += stalaktiteCount;
            source.StatsEffect.Dmg += stalaktiteCount * 10;
        }

        public override void CancelEffect(Hero source)
        {
            source.StatsEffect.Armor -= stalaktiteCount;
            source.StatsEffect.Resist -= stalaktiteCount;
            source.StatsEffect.Dmg -= stalaktiteCount * 10;
        }
    }
}
