using BattleArenaServer.Skills.ChaosSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class ChaosHero : Hero
    {
        public ChaosHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Chaos";


            SkillList[0] = new ChaosPowerPSkill(this);
            SkillList[1] = new ChaosStrikeSkill();
            SkillList[2] = new ChaosStormSkill();
            SkillList[3] = new BattleConfusionSkill();
            SkillList[4] = new HellJawsSkill();
            Respawn();
        }

        public override void Respawn()
        {
            MaxHP = HP = 1100;
            Armor = 0;
            Resist = 0;

            AP = 4;

            AttackRadius = 1;
            Dmg = 60;

            base.Respawn();
            (SkillList[0] as PassiveSkill).refreshEffect();
        }
    }
}
