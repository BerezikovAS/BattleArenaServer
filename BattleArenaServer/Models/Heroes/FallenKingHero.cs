using BattleArenaServer.Skills.FallenKingSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class FallenKingHero : Hero
    {
        public FallenKingHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Fallen King";

            Respawn();

            SkillList[0] = new ManaBanePSkill(this);
            SkillList[1] = new DepletionAbsorptionSkill();
            SkillList[2] = new LastStandSkill();
            SkillList[3] = new RoyalSacrificeSkill();
            SkillList[4] = new ParalysisSkill();
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
