using BattleArenaServer.Skills._SummonsSkills;
using BattleArenaServer.Skills;

namespace BattleArenaServer.Models.Summons
{
    public class HealingDroneSummon : Summon
    {
        public HealingDroneSummon(int Id, string Team, int casterId, int lifeTime, int HP, int armor, int resist, bool upgraded)
            : base(Id, Team, casterId, lifeTime)
        {
            Name = "Healing Drone";

            MaxHP = this.HP = HP;
            Armor = armor;
            Resist = resist;

            AP = 4;

            AttackRadius = 0;
            Dmg = 0;

            MoveSkill = new EmptySkill();
            SkillList[1] = new FlySkill(upgraded);
            SkillList[2] = new HealingSpraySkill(upgraded);

            this.AddEffect -= this.BaseAddEffect;
        }
    }
}
