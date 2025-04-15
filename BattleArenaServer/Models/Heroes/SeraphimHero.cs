using BattleArenaServer.Skills.SeraphimSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class SeraphimHero : Hero
    {
        public SeraphimHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Seraphim";

            Respawn();

            SkillList[0] = new SwiftnessPSkill(this);
            SkillList[1] = new BattleRushSkill();
            SkillList[2] = new GreateJudgementSkill();
            SkillList[3] = new SaintlyStrikeSkill();
            SkillList[4] = new GuardianAngelSkill();
        }

        public override void Respawn()
        {
            MaxHP = HP = 1000;
            Armor = 4;
            Resist = 4;

            AP = 4;

            AttackRadius = 1;
            Dmg = 105;

            base.Respawn();
        }
    }
}
