using BattleArenaServer.Skills.AeroturgSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class AeroturgHero : Hero
    {
        public AeroturgHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Aeroturg";

            MaxHP = HP = 850;
            Armor = 2;
            Resist = 5;

            AP = 4;

            AttackRadius = 3;
            Dmg = 105;

            SkillList[0] = new OverChargePSkill(this);
            SkillList[1] = new SwapSkill();
            SkillList[2] = new ChainLightningSkill();
            SkillList[3] = new ThunderWaveSkill();
            SkillList[4] = new AirFormSkill();
        }
    }
}
