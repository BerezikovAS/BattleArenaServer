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
            target.afterSpellCast += AfterSpellCastDelegate;
        }

        public override void CancelEffect(Hero source, GameData gameData)
        {
            foreach (var hero in gameData._heroes)
            {
                hero.afterSpellCast -= AfterSpellCastDelegate;
            }
        }

        private void AfterSpellCastDelegate(Hero attacker, Hero? defender, Skill skill, GameData gameData)
        {
            AttackService.SetDamage(source, attacker, dmg, Consts.DamageType.Magic, gameData);
        }
    }
}
