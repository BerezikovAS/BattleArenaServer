using BattleArenaServer.Skills.GolemSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class GolemHero : Hero
    {
        public GolemHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Golem";

            SkillList[0] = new StoneEndurancePSkill(this);
            SkillList[1] = new MagnetSkill();
            SkillList[2] = new IronBlastSkill();
            SkillList[3] = new ShockWaveSkill();
            SkillList[4] = new FortificationSkill();

            Respawn();
        }

        public override void Respawn()
        {
            MaxHP = HP = 1250;
            Armor = 8;
            Resist = 0;

            AP = 4;

            AttackRadius = 1;
            Dmg = 105;

            base.Respawn();
            (SkillList[0] as PassiveSkill).refreshEffect();
        }
    }
}
