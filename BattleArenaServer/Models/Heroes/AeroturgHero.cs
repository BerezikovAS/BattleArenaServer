using BattleArenaServer.Skills.AeroturgSkills;
using BattleArenaServer.Services;

namespace BattleArenaServer.Models.Heroes
{
    public class AeroturgHero : Hero
    {
        public AeroturgHero()
        {
            Id = 4;
            Name = "Aeroturg";
            Team = "red";

            MaxHP = HP = 850;
            Armor = 1;
            Resist = 4;

            UpgradePoints = 1;

            AP = 4;

            AttackRadius = 3;
            Dmg = 95;

            SkillList[0] = new SwapSkill();
            SkillList[1] = new ChainLightningSkill();
            SkillList[2] = new ThunderWaveSkill();
            SkillList[3] = new AirFormSkill();

            afterAttack += AfterAttackDelegate;
        }

        private bool AfterAttackDelegate(Hero attacker, Hero? defender, int dmg)
        {
            Hex? targetHex = GameData._hexes.FirstOrDefault(x => x.ID == defender?.HexId);
            if (targetHex != null && defender != null)
            {
                foreach (var hex in UtilityService.GetHexesRadius(targetHex, 1))
                {
                    if (hex.HERO != null && hex.HERO.Team != attacker.Team && hex.HERO.Id != defender.Id)
                    {
                        double splashDmg = Math.Round(dmg * 0.3);
                        AttackService.SetDamage(attacker, hex.HERO, (int)splashDmg, Consts.DamageType.Magic);
                    }
                }
            }

            return true;
        }
    }
}
