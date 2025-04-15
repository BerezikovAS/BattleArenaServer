using BattleArenaServer.Skills._SummonsSkills;

namespace BattleArenaServer.Models.Summons
{
    public class EagleSummon : Summon
    {
        public EagleSummon(int Id, string Team, int casterId, int lifeTime, int HP, int armor, int resist, int dmg)
            : base(Id, Team, casterId, lifeTime)
        {
            Name = "Eagle";

            MaxHP = this.HP = HP;
            Armor = armor;
            Resist = resist;

            AP = 4;

            AttackRadius = 1;
            Dmg = dmg;

            SkillList[1] = new FlySkill(true);
            SkillList[2] = new PeckOutSkill();
        }
    }
}
