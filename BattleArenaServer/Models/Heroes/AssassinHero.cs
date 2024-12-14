using BattleArenaServer.Skills.AssassinSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class AssassinHero : Hero
    {
        public AssassinHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Assassin";

            MaxHP = HP = 900;
            Armor = 4;
            Resist = 4;

            AP = 4;

            AttackRadius = 1;
            Dmg = 105;

            SkillList[0] = new LiquidationPSkill(this);
            SkillList[1] = new BlitzAttackSkill();
            SkillList[2] = new SmokeBombSkill();
            SkillList[3] = new RuptureSkill();
            SkillList[4] = new AdrenalinSkill();
        }
    }
}
