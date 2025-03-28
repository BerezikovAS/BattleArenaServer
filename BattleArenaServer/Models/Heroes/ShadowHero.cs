using BattleArenaServer.Skills.ShadowSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class ShadowHero : Hero
    {
        public ShadowHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Shadow";

            MaxHP = HP = 900;
            Armor = 5;
            Resist = 3;

            AP = 4;

            AttackRadius = 1;
            Dmg = 95;

            SkillList[0] = new ShadowsEverywherePSkill(this);
            SkillList[1] = new ShadowWalkSkill();
            SkillList[2] = new BladeOfDarknessSkill();
            SkillList[3] = new SuffocationSkill();
            SkillList[4] = new ShadowTwinSkill();
        }
    }
}
