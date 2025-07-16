using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Skills.SnowQueenSkills.Auras
{
    public class ChillingColdAura : Aura
    {
        int dmg_reduce = 0;
        List<Hero> affectedHeroes = new List<Hero>();
        public ChillingColdAura(bool upgraded, int dmg_reduce, int radius)
        {
            Name = "ChillingColdAura";
            this.radius = radius;
            type = Consts.AuraType.Continuous;
            this.dmg_reduce = dmg_reduce;
        }

        public override void SetEffect(Hero heroSource, Hex hexSource, GameData gameData)
        {
            foreach (var n in UtilityService.GetHexesRadius(hexSource, radius, gameData))
            {
                if (n.HERO != null && n.HERO.Team != heroSource.Team)
                {
                    ApplyEffect(heroSource, n.HERO, gameData);
                }
            }
        }

        public override void ApplyEffect(Hero source, Hero target, GameData gameData)
        {
            target.StatsEffect.DmgMultiplier -= Convert.ToDouble(dmg_reduce) / 100;
            affectedHeroes.Add(target);
        }

        public override void CancelEffect(Hero source, GameData gameData)
        {
            foreach (var hero in affectedHeroes)
            {
                hero.StatsEffect.DmgMultiplier += Convert.ToDouble(dmg_reduce) / 100;
            }
            affectedHeroes.Clear();
        }
    }
}
