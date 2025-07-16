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

        public override void SetEffect(Hero heroSource, Hex hexSource, GameData gameData)
        {
            stalaktiteCount = 0;
            foreach (var n in UtilityService.GetHexesRadius(hexSource, radius, gameData))
            {
                if (n.HERO != null && n.HERO.Name == "Stalaktite" && n.HERO.Team == heroSource.Team)
                {
                    stalaktiteCount++;
                    if (upgraded && hexSource.Distance(n) == 1)
                        stalaktiteCount++;
                }
            }
            ApplyEffect(heroSource, heroSource, gameData);
        }

        public override void ApplyEffect(Hero source, Hero target, GameData gameData)
        {
            source.Armor += stalaktiteCount;
            source.Resist += stalaktiteCount;
            source.Dmg += stalaktiteCount * 10;
        }

        public override void CancelEffect(Hero source, GameData gameData)
        {
            source.Armor -= stalaktiteCount;
            source.Resist -= stalaktiteCount;
            source.Dmg -= stalaktiteCount * 10;
            stalaktiteCount = 0;
        }
    }
}
