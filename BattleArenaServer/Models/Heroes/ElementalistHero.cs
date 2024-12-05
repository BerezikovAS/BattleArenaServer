using BattleArenaServer.Skills.ElementalistSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class ElementalistHero : Hero
    {
        public ElementalistHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Elementalist";

            MaxHP = HP = 875;
            Armor = 2;
            Resist = 4;

            AP = 4;

            AttackRadius = 3;
            Dmg = 100;

            SkillList[0] = new ElementEssencePSkill(this);
            SkillList[1] = new VentusSkill();
            SkillList[2] = new AquaSkill();
            SkillList[3] = new IgnisSkill();
            SkillList[4] = new TerraSkill();
        }
    }
}
