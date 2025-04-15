using BattleArenaServer.Skills.InvokerSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class InvokerHero : Hero
    {
        public InvokerHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Invoker";

            Respawn();

            SkillList[0] = new SpellRechargePSkill(this);
            SkillList[1] = new ArcaneBoltSkill();
            SkillList[2] = new ForstBreathSkill();
            SkillList[3] = new ManaBlastSkill();
            SkillList[4] = new TrickyEscapeSkill();
        }

        public override void Respawn()
        {
            MaxHP = HP = 850;
            Armor = 3;
            Resist = 3;

            AP = 4;

            AttackRadius = 3;
            Dmg = 100;

            base.Respawn();
        }
    }
}
