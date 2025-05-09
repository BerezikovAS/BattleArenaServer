﻿using BattleArenaServer.Skills.GeomantSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class GeomantHero : Hero
    {
        public GeomantHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Geomant";

            Respawn();

            SkillList[0] = new StoneStrenghtPSkill(this);
            SkillList[1] = new StalaktiteSkill();
            SkillList[2] = new GiantBoulderSkill();
            SkillList[3] = new StoneBloodSkill();
            SkillList[4] = new EarthquakeSkill();
        }

        public override void Respawn()
        {
            MaxHP = HP = 850;
            Armor = 2;
            Resist = 2;

            AP = 4;

            AttackRadius = 3;
            Dmg = 100;

            base.Respawn();
        }
    }
}
