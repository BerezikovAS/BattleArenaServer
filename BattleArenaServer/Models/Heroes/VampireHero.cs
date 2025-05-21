using BattleArenaServer.Skills.VampireSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class VampireHero : Hero
    {
        public VampireHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Vampire";

            Respawn();

            SkillList[0] = new ThirstPSkill(this);
            SkillList[1] = new SubjugationSkill();
            SkillList[2] = new CoverOfNightSkill();
            SkillList[3] = new SharpBladesSkill();
            SkillList[4] = new CoffinSkill();
        }

        public override void Respawn()
        {
            MaxHP = HP = 925;
            Armor = 4;
            Resist = 3;

            AP = 4;

            AttackRadius = 1;
            Dmg = 100;

            base.Respawn();
        }
    }
}
