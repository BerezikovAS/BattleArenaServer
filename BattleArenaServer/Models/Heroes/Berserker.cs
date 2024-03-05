using BattleArenaServer.Skills.Knight;

namespace BattleArenaServer.Models.Heroes
{
    public class Berserker : Hero
    {
        public Berserker()
        {
            Id = 2;
            Name = "Berserker";
            Team = "red";

            MaxHP = HP = 1000;
            Armor = 4;
            Resist = 2;

            AP = 4;

            AttackRadius = 1;
            Dmg = 105;

            SkillList[0] = new WhirlwindAxes();
            SkillList[1] = new HailStrike();
            SkillList[2] = new FireLine();
        }
    }
}
