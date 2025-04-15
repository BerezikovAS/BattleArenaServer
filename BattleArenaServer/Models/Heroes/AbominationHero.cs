using BattleArenaServer.Models.Items;
using BattleArenaServer.Skills.AbominationSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class AbominationHero : Hero
    {
        public AbominationHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Abomination";

            Respawn();

            SkillList[0] = new FleshEaterPSkill(this);
            SkillList[1] = new BloodCurseSkill();
            SkillList[2] = new BloodTransferSkill();
            SkillList[3] = new BloodRainSkill();
            SkillList[4] = new OssificationSkill();

            foreach (var item in Items)
                item.ApplyEffect(this);
        }

        public override void Respawn()
        {
            MaxHP = HP = 1500;
            Armor = 0;
            Resist = 0;

            AP = 4;

            AttackRadius = 1;
            Dmg = 100;

            base.Respawn();
        }
    }
}
