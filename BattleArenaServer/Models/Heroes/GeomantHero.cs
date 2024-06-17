using BattleArenaServer.Skills.AeroturgSkills;
using BattleArenaServer.Skills.GeomantSkills;
using BattleArenaServer.Skills.GeomantSkills.Auras;

namespace BattleArenaServer.Models.Heroes
{
    public class GeomantHero : Hero
    {
        public GeomantHero()
        {
            Id = 5;
            Name = "Geomant";
            Team = "blue";

            MaxHP = HP = 850;
            Armor = 1;
            Resist = 1;

            UpgradePoints = 1;

            AP = 4;

            AttackRadius = 3;
            Dmg = 90;

            SkillList[0] = new StalaktiteSkill();
            SkillList[1] = new GiantBoulderSkill();
            SkillList[2] = new StoneBloodSkill();
            SkillList[3] = new EarthquakeSkill();

            AuraList.Add(new StoneStrengthAura());
        }
    }
}
