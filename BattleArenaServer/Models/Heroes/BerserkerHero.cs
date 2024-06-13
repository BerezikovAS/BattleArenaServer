﻿using BattleArenaServer.Services;
using BattleArenaServer.Skills.BerserkerSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class BerserkerHero : Hero
    {
        public BerserkerHero()
        {
            Id = 2;
            Name = "Berserker";
            Team = "red";

            MaxHP = HP = 1000;
            Armor = 4;
            Resist = 2;

            AP = 4;

            UpgradePoints = 1;

            AttackRadius = 1;
            Dmg = 105;

            SkillList[0] = new WhirlwindAxesSkill();
            SkillList[1] = new BrokenLegSkill();
            SkillList[2] = new BattleCrySkill();
            SkillList[3] = new BloodRageSkill();

            passiveAttackDamage += BattleTrance;
        }

        private int BattleTrance(Hero attacker, Hero? defender)
        {
            Hex? attackerHex = GameData._hexes.FirstOrDefault(x => x.HERO?.Id == attacker.Id);

            int enemiesCount = 0;
            double extraDmg = 0;
            if (attackerHex != null)
            {
                foreach (var n in UtilityService.GetHexesRadius(attackerHex, 1))
                {
                    if (n.HERO != null && n.HERO.Team != attacker.Team)
                        enemiesCount++;
                }
                extraDmg = (attacker.Dmg + attacker.StatsEffect.Dmg) * ((enemiesCount - 1) * 0.2);
                return (int)(Math.Round(extraDmg));
            }
            return 0;
        }
    }
}