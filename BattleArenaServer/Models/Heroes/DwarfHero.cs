using BattleArenaServer.Skills.DwarfSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class DwarfHero : Hero
    {
        public DwarfHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Dwarf";

            Respawn();

            SkillList[0] = new ToughnessPSkill(this);
            SkillList[1] = new EngageSkill();
            SkillList[2] = new RuneHammerSkill();
            SkillList[3] = new ThunderclapSkill();
            SkillList[4] = new TauntSkill();
        }

        public override void Respawn()
        {
            MaxHP = HP = 1100;
            Armor = 4;
            Resist = 3;

            AP = 4;

            AttackRadius = 1;
            Dmg = 100;

            base.Respawn();
        }
    }
}
