using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.Crossbowman
{
    public class LongShotPSkill : Skill
    {
        public LongShotPSkill()
        {
            name = "Long Shot";
            title = "Чем дальше цель атаки, тем больше урона она получит. +10% к урону за каждую клетку между Вами и целью.";
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
