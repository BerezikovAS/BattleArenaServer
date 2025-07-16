using BattleArenaServer.Skills.ArtistSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class ArtistHero : Hero
    {
        public ArtistHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Artist";

            Respawn();

            SkillList[0] = new CombatThemePSkill(this);
            SkillList[1] = new VerseSkill();
            SkillList[2] = new ChorusSkill();
            SkillList[3] = new ClimaxSkill();
            SkillList[4] = new TheFinalSkill();
        }

        public override void Respawn(GameData? gameData = null)
        {
            base.Respawn(gameData);

            MaxHP = HP = 800;
            Armor = 3;
            Resist = 3;

            AP = 4;

            AttackRadius = 2;
            Dmg = 100;

            ApplyItems(gameData);
        }
    }
}
