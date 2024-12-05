using BattleArenaServer.Skills.Crossbowman;
using BattleArenaServer.Skills.CultistSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class CultistHero : Hero
    {
        public CultistHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Cultist";

            MaxHP = HP = 875;
            Armor = 1;
            Resist = 4;

            AP = 4;

            AttackRadius = 2;
            Dmg = 100;

            SkillList[0] = new ScornPSkill(this);
            SkillList[1] = new DecayCurseSkill();
            SkillList[2] = new WeakeningWaveSkill();
            SkillList[3] = new CorruptionSkill();
            SkillList[4] = new RitualSkill();
        }
    }
}
