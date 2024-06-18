using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.BerserkerSkills
{
    public class BattleTrancePSkill : Skill
    {
        public BattleTrancePSkill()
        {
            name = "Battle Trance";
            title = "Чем больше врагов вокруг, тем яростнее атаки. +20% к урону от атак за каждого врага вокруг, кроме первого.";
            skillType = Consts.SkillType.Passive;
        }

        public override bool Cast(RequestData requestData)
        {
            return false;
        }

        public override bool UpgradeSkill()
        {
            return false;
        }
    }
}
