using BattleArenaServer.Skills.DruidSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class DruidHero : Hero
    {
        public DruidHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Druid";

            SkillList[0] = new AdaptiveArmorPSkill(this);
            SkillList[1] = new WolfSkill();
            SkillList[2] = new PrimalFurySkill();
            SkillList[3] = new PoisonousSporesSkill();
            SkillList[4] = new NatureBalanceSkill();

            Respawn();
        }

        public override void Respawn()
        {
            (SkillList[0] as PassiveSkill).refreshEffect();

            MaxHP = HP = 875;
            Armor = 2;
            Resist = 2;

            AP = 4;

            AttackRadius = 3;
            Dmg = 95;

            base.Respawn();
        }
    }
}
