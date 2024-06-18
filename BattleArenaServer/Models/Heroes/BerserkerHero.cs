using BattleArenaServer.Services;
using BattleArenaServer.Skills.BerserkerSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class BerserkerHero : Hero
    {
        public BerserkerHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Berserker";

            MaxHP = HP = 1000;
            Armor = 4;
            Resist = 2;

            AP = 4;

            UpgradePoints = 1;

            AttackRadius = 1;
            Dmg = 105;

            SkillList[0] = new BattleTrancePSkill(this);
            SkillList[1] = new WhirlwindAxesSkill();
            SkillList[2] = new BrokenLegSkill();
            SkillList[3] = new BattleCrySkill();
            SkillList[4] = new BloodRageSkill();
        }
    }
}
