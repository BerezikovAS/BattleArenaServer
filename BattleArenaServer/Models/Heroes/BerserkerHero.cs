using BattleArenaServer.Skills.BerserkerSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class BerserkerHero : Hero
    {
        public BerserkerHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Berserker";

            MaxHP = HP = 1000;
            Armor = 5;
            Resist = 3;

            AP = 4;

            AttackRadius = 1;
            Dmg = 112;

            SkillList[0] = new BattleTrancePSkill(this);
            SkillList[1] = new WhirlwindAxesSkill();
            SkillList[2] = new BrokenLegSkill();
            SkillList[3] = new BattleCrySkill();
            SkillList[4] = new BloodRageSkill();
        }
    }
}
