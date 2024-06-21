using BattleArenaServer.Skills.Knight;
using BattleArenaServer.Skills.Priest;
using BattleArenaServer.Skills.PriestSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class AngelHero : Hero
    {
        public AngelHero(int Id, string Team) : base(Id, Team)
        {
            Id = 1;
            Name = "Angel";
            Team = "blue";

            MaxHP = HP = 1000;
            Armor = 2;
            Resist = 3;

            AP = 4;
            
            AttackRadius = 1;
            Dmg = 100;

            SkillList[0] = new SecondBreathSkill();
            SkillList[1] = new SmightSkill();
            SkillList[2] = new CondemnationSkill();
        }
    }
}
