﻿using BattleArenaServer.Skills.Priest;
using BattleArenaServer.Skills.PriestSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class PriestHero : Hero
    {
        public PriestHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Priest";

            MaxHP = HP = 1000;
            Armor = 2;
            Resist = 3;

            AP = 4;

            AttackRadius = 1;
            Dmg = 94;

            SkillList[0] = new BlessAuraPSkill(this);
            SkillList[1] = new BlindingLightSkill();
            SkillList[2] = new SmightSkill();
            SkillList[3] = new RestorationSkill();
            SkillList[4] = new CondemnationSkill();
        }
    }
}
