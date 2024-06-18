using BattleArenaServer.Services;
using BattleArenaServer.Skills.BerserkerSkills;
using BattleArenaServer.Skills.Crossbowman;
using System;

namespace BattleArenaServer.Models.Heroes
{
    public class CrossbowmanHero : Hero
    {
        public CrossbowmanHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Crossbowman";

            MaxHP = HP = 875;
            Armor = 2;
            Resist = 2;

            AP = 4;

            UpgradePoints = 1;

            AttackRadius = 3;
            Dmg = 80;

            SkillList[0] = new LongShotPSkill();
            SkillList[1] = new EagleEyeSkill();
            SkillList[2] = new CaltropSkill();
            SkillList[3] = new SharpFangSkill();
            SkillList[4] = new PinDownSkill();

            passiveAttackDamage += LongShot;
        }

        private int LongShot(Hero attacker, Hero? defender)
        {
            if (defender == null)
                return 0;

            Hex? attackerHex = GameData._hexes.FirstOrDefault(x => x.HERO?.Id == attacker.Id);
            Hex? defenderHex = GameData._hexes.FirstOrDefault(x => x.HERO?.Id == defender.Id);

            if (attackerHex != null && defenderHex != null)
            {
                double extraDmg = (attacker.Dmg + attacker.StatsEffect.Dmg) * 0.1 * (attackerHex.Distance(defenderHex) - 1);
                return (int)(Math.Round(extraDmg));
            }
            return 0;
        }
    }
}
