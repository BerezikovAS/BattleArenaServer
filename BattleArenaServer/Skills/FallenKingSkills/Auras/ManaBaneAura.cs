using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Skills.FallenKingSkills.Auras
{
    public class ManaBaneAura : Aura
    {
        int dmg = 0;
        Hero source;
        public ManaBaneAura(int dmg, int radius, Hero source)
        {
            Name = "ManaBaneAura";
            this.radius = radius;
            type = Consts.AuraType.Continuous;
            this.dmg = dmg;
            this.source = source;
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
            target.afterSpellCast += AfterSpellCastDelegate;
        }

        public override void CancelEffect(Hero source)
        {
            foreach (var hero in GameData._heroes)
            {
                hero.afterSpellCast -= AfterSpellCastDelegate;
            }
        }

        private void AfterSpellCastDelegate(Hero attacker, Hero? defender, Skill skill)
        {
            AttackService.SetDamage(source, attacker, dmg, Consts.DamageType.Magic);
        }
    }
}
