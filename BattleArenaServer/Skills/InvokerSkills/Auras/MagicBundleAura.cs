using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Skills.InvokerSkills.Auras
{
    public class MagicBundleAura : Aura
    {
        int reduce_resist = 0;
        public MagicBundleAura(bool upgraded, int reduce_resist)
        {
            Name = "MagicBundleAura";
            radius = 1;
            type = Consts.AuraType.Continuous;
            this.reduce_resist = reduce_resist;
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
            target.passiveResistance += ReduceResist;
        }

        public override void CancelEffect(Hero source, GameData gameData)
        {
            foreach (var hero in gameData._heroes)
            {
                hero.passiveResistance -= ReduceResist;
            }
        }

        private int ReduceResist(Hero? attacker, Hero defender)
        {
            return -1 * reduce_resist;
        }
    }
}
