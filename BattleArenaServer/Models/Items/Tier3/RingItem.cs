using BattleArenaServer.Services;

namespace BattleArenaServer.Models.Items.Tier3
{
    public class RingItem : Item
    {
        int resistPiercing = 3;
        int dmg = 40;

        public RingItem()
        {
            Name = "Ring";
            Amount = 1;
            Cost = 55;
            Description = $"Атаки и способности игнорируют {resistPiercing} сопротивления.\n" +
                $"Каждое применение заклинания провоцирует ударную волну вокруг Вас, которое наносит {dmg} маг. урона";
            Level = 3;
            SellCost = 27;
        }

        public override void ApplyEffect(Hero hero)
        {
            hero.resistPiercing += ResistPiercingDelegate;
            hero.afterSpellCast += AfterSpellCast;
        }

        public override void RemoveEffect(Hero hero)
        {
            hero.resistPiercing -= ResistPiercingDelegate;
            hero.afterSpellCast -= AfterSpellCast;
        }

        private int ResistPiercingDelegate(Hero attacker, Hero defender)
        {
            return resistPiercing;
        }

        private void AfterSpellCast(Hero caster, Hero? target, Skill skill)
        {
            Hex? casterHex = GameData._hexes.FirstOrDefault(x => x.HERO != null && x.HERO.Id == caster.Id);
            if (casterHex != null)
            {
                foreach (var hex in UtilityService.GetHexesRadius(casterHex, 1))
                {
                    if (hex.HERO != null && hex.HERO.Team != caster.Team)
                        AttackService.SetDamage(caster, hex.HERO, dmg, Consts.DamageType.Magic);
                }
            }
        }
    }
}
