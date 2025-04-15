using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Skills.NecromancerSkills.Auras
{
    public class DeadlyAura : Aura
    {
        int reduce_resist = 0;
        public DeadlyAura(bool upgraded, int reduce_resist, int radius)
        {
            Name = "DeadlyAura";
            this.radius = radius;
            type = Consts.AuraType.Continuous;
            this.reduce_resist = reduce_resist;
        }

        public override void SetEffect(Hero heroSource, Hex hexSource)
        {
            foreach (var n in UtilityService.GetHexesRadius(hexSource, radius))
            {
                if (n.HERO != null && n.HERO.Team != heroSource.Team)
                {
                    ApplyEffect(heroSource, n.HERO);
                }
            }
        }

        public override void ApplyEffect(Hero source, Hero target)
        {
            target.passiveResistance += ReduceResist;
        }

        public override void CancelEffect(Hero source)
        {
            foreach (var hero in GameData._heroes)
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
