using BattleArenaServer.Skills.LizardmanSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class LizardmanHero : Hero
    {
        public LizardmanHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Lizardman";

            Respawn();

            SkillList[0] = new PoisonedWeaponPSkill(this);
            SkillList[1] = new LizardSkinSkill();
            SkillList[2] = new SwiftFeintSkill();
            SkillList[3] = new BodyHealingSkill();
            SkillList[4] = new DodgeSkill();
        }

        public override void Respawn(GameData? gameData = null)
        {
            base.Respawn(gameData);

            MaxHP = HP = 950;
            Armor = 4;
            Resist = 4;

            AP = 4;

            AttackRadius = 1;
            Dmg = 105;

            ApplyItems(gameData);
        }
    }
}
