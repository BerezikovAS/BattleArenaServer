using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.GeomantSkills
{
    public class StoneStrenghtPSkill : Skill
    {
        public StoneStrenghtPSkill()
        {
            name = "Stone Strenght";
            title = "Герой наполняется силой природы. Каждый столоктит в радиусе 3-х гексов даёт +1 к броне, +1 к сопротивлению, +10 к урону.";
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
