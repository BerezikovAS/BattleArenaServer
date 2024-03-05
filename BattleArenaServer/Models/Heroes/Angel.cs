using BattleArenaServer.Skills.Knight;
using BattleArenaServer.Skills.Priest;

namespace BattleArenaServer.Models.Heroes
{
    public class Angel : Hero
    {
        public Angel() {
            Id = 1;
            Name = "Angel";
            Team = "blue";

            MaxHP = HP = 1000;
            Armor = 2;
            Resist = 3;

            AP = 4;
            
            AttackRadius = 1;
            Dmg = 100;

            SkillList[0] = new SelfHeal();
            SkillList[1] = new Smight();
        }
    }
}
