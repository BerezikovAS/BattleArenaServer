using BattleArenaServer.Skills.NecromancerSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class NecromancerHero : Hero
    {
        public NecromancerHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Necromancer";

            Respawn();

            SkillList[0] = new DeadlyAuraPSkill(this);
            SkillList[1] = new LivingDeadSkill();
            SkillList[2] = new LifeDrainSkill();
            SkillList[3] = new ChainOfPainSkill();
            SkillList[4] = new GraveHandsSkill();
        }

        public override void Respawn(GameData? gameData = null)
        {
            base.Respawn(gameData);

            MaxHP = HP = 800;
            Armor = 2;
            Resist = 3;

            AP = 4;

            AttackRadius = 3;
            Dmg = 100;

            ApplyItems(gameData);
        }
    }
}
