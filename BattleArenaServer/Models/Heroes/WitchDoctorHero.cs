using BattleArenaServer.Skills.WitchDoctorSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class WitchDoctorHero : Hero
    {
        public WitchDoctorHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Witch Doctor";

            MaxHP = HP = 850;
            Armor = 2;
            Resist = 3;

            AP = 4;

            AttackRadius = 2;
            Dmg = 100;

            SkillList[0] = new PerfectHealthPSkill(this);
            SkillList[1] = new HealingHerbsSkill();
            SkillList[2] = new PurificationSkill();
            SkillList[3] = new VoodooPuppetSkill();
            SkillList[4] = new WitchTotemSkill();
        }
    }
}
