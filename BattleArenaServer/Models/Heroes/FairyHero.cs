using BattleArenaServer.Skills.FairySkills;

namespace BattleArenaServer.Models.Heroes
{
    public class FairyHero : Hero
    {
        public FairyHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Fairy";

            MaxHP = HP = 700;
            Armor = 2;
            Resist = 6;

            AP = 4;

            AttackRadius = 3;
            Dmg = 95;

            SkillList[0] = new CharmPSkill(this);
            SkillList[1] = new RebirthSkill();
            SkillList[2] = new MagicShieldSKill();
            SkillList[3] = new AstralPortalSkill();
            SkillList[4] = new PixiePowderSkill();
        }
    }
}
