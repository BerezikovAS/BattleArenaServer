using BattleArenaServer.Services;

namespace BattleArenaServer.Models.Items.Auras
{
    public class BattleBannerAura : Aura
    {
        int armorResist;
        int dmg;
        List<Hero> heroList = new List<Hero>();

        public BattleBannerAura(int armorResist, int dmg, int radius)
        {
            Name = "BattleBanner";
            this.radius = radius;
            type = Consts.AuraType.Continuous;
            this.armorResist = armorResist;
            this.dmg = dmg;
        }

        public override void SetEffect(Hero heroSource, Hex hexSource)
        {
            heroList.Clear();
            foreach (var n in UtilityService.GetHexesRadius(hexSource, radius))
            {
                if (n.HERO != null && n.HERO.Team == heroSource.Team)
                {
                    heroList.Add(n.HERO);
                    ApplyEffect(heroSource, n.HERO);
                }
            }
        }

        public override void ApplyEffect(Hero source, Hero target)
        {
            target.Armor += armorResist;
            target.Resist += armorResist;
            target.Dmg += dmg;
        }

        public override void CancelEffect(Hero source)
        {
            foreach (var hero in heroList)
            {
                hero.Armor -= armorResist;
                hero.Resist -= armorResist;
                hero.Dmg -= dmg;
            }
            heroList.Clear();
        }
    }
}
