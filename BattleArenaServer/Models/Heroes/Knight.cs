using BattleArenaServer.Interfaces;
using BattleArenaServer.Skills.Knight;

namespace BattleArenaServer.Models.Heroes
{
    public class Knight : Hero
    {
        public Knight() {
            Id = 0;
            Name = "Knight";
            Team = "blue";

            MaxHP = HP = 1000;
            Armor = 6;
            Resist = 3;

            AP = 4;

            AttackRadius = 3;
            Dmg = 105;

            SkillList[0] = new SelfHeal();
        }

        //public Skill Spell1 = new SelfHeal();
    }
}
