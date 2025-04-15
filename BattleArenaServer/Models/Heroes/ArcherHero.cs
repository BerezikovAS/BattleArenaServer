using BattleArenaServer.Skills.Crossbowman;

namespace BattleArenaServer.Models.Heroes
{
    public class ArcherHero : Hero
    {
        public ArcherHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Archer";

            Respawn();

            SkillList[0] = new LongShotPSkill(this);
            SkillList[1] = new EagleEyeSkill();
            SkillList[2] = new CaltropSkill();
            SkillList[3] = new SharpFangSkill();
            SkillList[4] = new PinDownSkill();
        }

        public override void Respawn()
        {
            MaxHP = HP = 875;
            Armor = 3;
            Resist = 3;

            AP = 4;

            AttackRadius = 3;
            Dmg = 102;

            base.Respawn();
        }
    }
}
