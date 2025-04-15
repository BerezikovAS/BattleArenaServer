using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Skills.GeomantSkills.Auras
{
    public class StoneStrengthAura : Aura
    {
        int stalaktiteCount = 0;
        bool upgraded = false;
        public StoneStrengthAura(bool upgraded)
        {
            Name = "StoneStrengthAura";
            radius = 3;
            type = Consts.AuraType.Continuous;
            this.upgraded = upgraded;
        }

        public override void SetEffect(Hero heroSource, Hex hexSource)
        {
            stalaktiteCount = 0;
            foreach (var n in UtilityService.GetHexesRadius(hexSource, radius))
            {
                if (n.HERO != null && n.HERO.Name == "Stalaktite")
                {
                    stalaktiteCount++;
                    if (upgraded && hexSource.Distance(n) == 1)
                        stalaktiteCount++;
                }
            }
            ApplyEffect(heroSource, heroSource);
        }

        public override void ApplyEffect(Hero source, Hero target)
        {
            source.Armor += stalaktiteCount;
            source.Resist += stalaktiteCount;
            source.Dmg += stalaktiteCount * 10;
        }

        public override void CancelEffect(Hero source)
        {
            source.Armor -= stalaktiteCount;
            source.Resist -= stalaktiteCount;
            source.Dmg -= stalaktiteCount * 10;
            stalaktiteCount = 0;
        }
    }
}
