namespace BattleArenaServer.Models
{
    public abstract class PassiveSkill : Skill
    {
        public PassiveSkill(Hero hero)
        {
            skillType = Consts.SkillType.Passive;
            this.hero = hero;
        }

        public Hero hero { get; set; }
    }
}
