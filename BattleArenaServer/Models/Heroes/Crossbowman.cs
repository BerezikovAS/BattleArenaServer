using BattleArenaServer.Skills.BerserkerSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class Crossbowman : Hero
    {
        public Crossbowman()
        {
            Id = 4;
            Name = "Crossbowman";
            Team = "blue";

            MaxHP = HP = 875;
            Armor = 2;
            Resist = 2;

            AP = 4;

            UpgradePoints = 1;

            AttackRadius = 3;
            Dmg = 80;

            SkillList[0] = new WhirlwindAxes();
            SkillList[1] = new BrokenLeg();
            SkillList[2] = new BattleCry();
            SkillList[3] = new BloodRage();

            //passiveAttackDamage += BattleTrance;
        }
    }
}
