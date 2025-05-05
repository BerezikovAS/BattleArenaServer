using BattleArenaServer.Skills.GhostSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class GhostHero : Hero
    {
        public GhostHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Ghost";

            Respawn();

            SkillList[0] = new EtherealityPSkill(this);
            SkillList[1] = new SpiritLinkSkill();
            SkillList[2] = new SwirlSkill();
            SkillList[3] = new DisappearSkill();
            SkillList[4] = new FearSkill();
        }

        public override void Respawn()
        {
            MaxHP = HP = 700;
            Armor = 0;
            Resist = 4;

            AP = 4;

            AttackRadius = 2;
            Dmg = 100;

            base.Respawn();
        }
    }
}
