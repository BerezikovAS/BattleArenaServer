﻿using BattleArenaServer.Skills.Knight;
using BattleArenaServer.Skills.KnightSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class KnightHero : Hero
    {
        public KnightHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Knight";

            MaxHP = HP = 1000;
            Armor = 6;
            Resist = 3;

            AP = 4;

            AttackRadius = 1;
            Dmg = 110;

            SkillList[0] = new HighShieldPSkill(this);
            SkillList[1] = new SecondBreathSkill();
            SkillList[2] = new ShieldBashSkill();
            SkillList[3] = new BodyGuardSkill();
            SkillList[4] = new RetributionSkill();
        }
    }
}
