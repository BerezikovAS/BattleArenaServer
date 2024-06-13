using BattleArenaServer.Skills.Priest;
using BattleArenaServer.Skills.PriestSkills;
using BattleArenaServer.Skills.PriestSkills.Auras;

namespace BattleArenaServer.Models.Heroes
{
    public class PriestHero : Hero
    {
        public PriestHero()
        {
            Id = 3;
            Name = "Priest";
            Team = "red";

            MaxHP = HP = 1000;
            Armor = 2;
            Resist = 3;

            UpgradePoints = 1;

            AP = 4;

            AttackRadius = 1;
            Dmg = 94;

            SkillList[0] = new BlindingLightSkill();
            SkillList[1] = new SmightSkill();
            SkillList[2] = new RestorationSkill();
            SkillList[3] = new CondemnationSkill();

            AuraList.Add(new BlessAura());
        }
    }
}
