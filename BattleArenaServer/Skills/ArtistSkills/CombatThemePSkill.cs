using BattleArenaServer.Models;
using BattleArenaServer.Models.Obstacles;
using BattleArenaServer.Services;

namespace BattleArenaServer.Skills.ArtistSkills
{
    public class CombatThemePSkill : PassiveSkill
    {
        private int heal = 15;
        public CombatThemePSkill(Hero hero) : base(hero)
        {
            name = "Combat Theme";
            title = $"Боевая музыка воодушевляет отряд. Ваши атаки и способности излечивают союзников на {heal} ХП в радиусе 2-х клеток.";
            titleUpg = "+7 к лечению";
            hero.afterSpellCast += CombatThemeAfterSpellCast;
            hero.afterAttack += CombatThemeAfterAttack;
        }

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            return false;
        }

        public override void refreshEffect(GameData gameData)
        {
        }

        public override bool UpgradeSkill(GameData gameData)
        {
            if (!upgraded)
            {
                upgraded = true;
                heal += 7;
                title = $"Боевая музыка воодушевляет отряд. Ваши атаки и способности излечивают союзников на {heal} ХП в радиусе 2-х клеток.";
                return true;
            }
            return false;
        }

        private void CombatThemeAfterSpellCast(Hero caster, Hero? target, Skill skill, GameData gameData)
        {
            if (caster.HP > 0)
                Heal(caster, gameData);
        }

        private bool CombatThemeAfterAttack(Hero attacker, Hero defender, int dmg, Consts.DamageType dmgType, GameData gameData)
        {
            if (attacker.HP > 0)
                Heal(attacker, gameData);
            return true;
        }

        private void Heal(Hero caster, GameData gameData)
        {
            Hex? hex = gameData._hexes.FirstOrDefault(x => x.HERO != null && x.HERO.Id == caster.Id);
            if (hex == null)
                return;
            foreach (Hex n in UtilityService.GetHexesRadius(hex, 2, gameData))
            {
                if (n.HERO != null && n.HERO.Team == hero.Team && n.HERO is not SolidObstacle)
                    n.HERO.Heal(heal);
            }
        }
    }
}
