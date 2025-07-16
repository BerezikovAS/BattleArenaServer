using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Skills.AeroturgSkills
{
    public class OverChargePSkill : PassiveSkill
    {
        Consts.DamageType damageType = Consts.DamageType.Magic;
        string damageTypeName = "магический";
        public OverChargePSkill(Hero hero) : base(hero)
        {
            name = "Overcharge";
            title = $"Атаки героя электризуют врага, нанося ему и соседним врагам {damageTypeName} урон в размере 30% от наносимого атакой урона.";
            titleUpg = "Урон по области становится чистым.";
            hero.afterAttack += AfterAttackDelegate;
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
                hero.afterAttack -= AfterAttackDelegate;
                damageType = Consts.DamageType.Pure;
                damageTypeName = "чистый";
                hero.afterAttack += AfterAttackDelegate;
                title = $"Атаки героя электризуют врага, нанося ему и соседним врагам {damageTypeName} урон в размере 30% от наносимого атакой урона.";
                return true;
            }
            return false;
        }

        private bool AfterAttackDelegate(Hero attacker, Hero? defender, int dmg, Consts.DamageType dmgType, GameData gameData)
        {
            Hex? targetHex = gameData._hexes.FirstOrDefault(x => x.ID == defender?.HexId);
            if (targetHex != null && defender != null)
            {
                foreach (var hex in UtilityService.GetHexesRadius(targetHex, 1, gameData))
                {
                    if (hex.HERO != null && hex.HERO.Team != attacker.Team && hex.HERO.Id != defender.Id)
                    {
                        double splashDmg = Math.Round(dmg * 0.3);
                        AttackService.SetDamage(attacker, hex.HERO, (int)splashDmg, damageType, gameData);
                    }
                }
            }

            return true;
        }
    }
}
