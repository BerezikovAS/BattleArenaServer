using BattleArenaServer.Skills.TinkerSkill;

namespace BattleArenaServer.Models.Heroes
{
    public class TinkerHero : Hero
    {
        public TinkerHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Tinker";

            MaxHP = HP = 900;
            Armor = 5;
            Resist = 2;

            AP = 4;

            AttackRadius = 1;
            Dmg = 100;

            SkillList[0] = new EmergencyShieldPSkill(this);
            SkillList[1] = new FlameTurretSkill();
            SkillList[2] = new HealingDroneSkill();
            SkillList[3] = new CopperCageSkill();
            SkillList[4] = new SteamStrikeSkill();
        }
    }
}
