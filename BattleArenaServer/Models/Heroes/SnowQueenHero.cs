using BattleArenaServer.Skills.SnowQueenSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class SnowQueenHero : Hero
    {
        public SnowQueenHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Snow Queen";

            Respawn();

            SkillList[0] = new ChillingColdPSkill(this);
            SkillList[1] = new MirrorShieldSkill();
            SkillList[2] = new FrostBallSkill();
            SkillList[3] = new IceShardSkill();
            SkillList[4] = new DeepFreezeSkill();
        }

        public override void Respawn()
        {
            MaxHP = HP = 800;
            Armor = 2;
            Resist = 4;

            AP = 4;

            AttackRadius = 3;
            Dmg = 100;

            base.Respawn();
        }
    }
}
