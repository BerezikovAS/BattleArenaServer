using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.KnightSkills
{
    public class HighShieldPSkill : Skill
    {
        public HighShieldPSkill()
        {
            name = "High Shield";
            title = "Герой защищает себя и союзников рядом от дальнобойных атак. При обороне имеет +2 брони, если атакующий находится не ближе двух клеток от цели атаки.";
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
