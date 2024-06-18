using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.AeroturgSkills
{
    public class OverChargePSkill : Skill
    {
        public OverChargePSkill()
        {
            name = "Overcharge";
            title = "Атаки героя электризуют врага, нанося ему и соседним врагам магический урон в размере 30% от наносимого атакой урона.";
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
