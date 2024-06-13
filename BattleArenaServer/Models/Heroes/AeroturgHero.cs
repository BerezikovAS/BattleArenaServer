using BattleArenaServer.Skills.Priest;
using BattleArenaServer.Skills.PriestSkills.Auras;
using BattleArenaServer.Skills.PriestSkills;
using BattleArenaServer.Skills.AeroturgSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class AeroturgHero : Hero
    {
        public AeroturgHero()
        {
            Id = 4;
            Name = "Aeroturg";
            Team = "red";

            MaxHP = HP = 850;
            Armor = 1;
            Resist = 4;

            UpgradePoints = 1;

            AP = 4;

            AttackRadius = 3;
            Dmg = 95;

            SkillList[0] = new SwapSkill();
            SkillList[1] = new ChainLightningSkill();
            SkillList[2] = new ThunderWaveSkill();
            SkillList[3] = new CondemnationSkill();

            AuraList.Add(new BlessAura());
        }
    }
}
