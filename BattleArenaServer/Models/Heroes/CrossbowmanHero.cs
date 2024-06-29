﻿using BattleArenaServer.Skills.Crossbowman;

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

            AttackRadius = 4;
            Dmg = 95;

            SkillList[0] = new LongShotPSkill(this);
            SkillList[1] = new EagleEyeSkill();
            SkillList[2] = new CaltropSkill();
            SkillList[3] = new SharpFangSkill();
            SkillList[4] = new PinDownSkill();
        }
    }
}
