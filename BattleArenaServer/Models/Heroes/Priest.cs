using BattleArenaServer.Skills.Priest;
using BattleArenaServer.Skills.PriestSkills;
using BattleArenaServer.Skills.PriestSkills.Auras;

namespace BattleArenaServer.Models.Heroes
{
    public class Priest : Hero
    {
        public Priest()
        {
            Id = 3;
            Name = "Priest";
            Team = "red";

            MaxHP = HP = 1000;
            Armor = 3;
            Resist = 3;

            AP = 4;

            AttackRadius = 2;
            Dmg = 94;

            SkillList[0] = new BlindingLight();
            SkillList[1] = new Smight();
            SkillList[2] = new Restoration();
            SkillList[3] = new Condemnation();

            AuraList.Add(new BlessAura());
        }
    }
}
