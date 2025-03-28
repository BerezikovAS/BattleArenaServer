using BattleArenaServer.Skills.GuardianSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class GuardianHero : Hero
    {
        public GuardianHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Guardian";

            MaxHP = HP = 1000;
            Armor = 5;
            Resist = 3;

            AP = 4;

            AttackRadius = 1;
            Dmg = 105;

            SkillList[0] = new EncouragePSkill(this);
            SkillList[1] = new OnslaughtSkill();
            SkillList[2] = new SpearPiercingSkill();
            SkillList[3] = new BattleOrderSkill();
            SkillList[4] = new DisarmStrikeSkill();
        }
    }
}
