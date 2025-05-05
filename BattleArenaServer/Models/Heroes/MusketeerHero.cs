using BattleArenaServer.Skills.MusketeerSKills;

namespace BattleArenaServer.Models.Heroes
{
    public class MusketeerHero : Hero
    {
        public MusketeerHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Musketeer";

            Respawn();

            SkillList[0] = new GunPowerPSkill(this);
            SkillList[1] = new GrenadeSkill();
            SkillList[2] = new BayonetSkill();
            SkillList[3] = new TakeAimSkill();
            SkillList[4] = new RiposteSkill();
        }

        public override void Respawn()
        {
            MaxHP = HP = 825;
            Armor = 2;
            Resist = 2;

            AP = 4;

            AttackRadius = 3;
            Dmg = 105;

            base.Respawn();
        }
    }
}
