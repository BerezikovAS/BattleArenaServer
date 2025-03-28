using BattleArenaServer.Effects;
using BattleArenaServer.Skills;
using BattleArenaServer.Skills.TinkerSkill.SummonsSkills;

namespace BattleArenaServer.Models.Summons
{
    public class FlameTurretSummon : Summon
    {
        public FlameTurretSummon(int Id, string Team, int casterId, int lifeTime, int HP, int armor, int resist, bool upgraded)
            : base(Id, Team, casterId, lifeTime)
        {
            Name = "Flame Turret";

            MaxHP = this.HP = HP;
            Armor = armor;
            Resist = resist;

            AP = 4;

            AttackRadius = upgraded ? 2 : 0;
            Dmg = upgraded ? 50 : 0;

            MoveSkill = new EmptySkill();
            SkillList[1] = new BurnSkill();

            this.AddEffect -= this.BaseAddEffect;
        }
    }
}
