using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.PriestSkills
{
    public class BlessAuraPSkill : Skill
    {
        public BlessAuraPSkill()
        {
            name = "Bless Aura";
            title = "Святое благословение излечивает героя и союзников рядом на 5% потерянного здоровья.\n" +
                "Лечение усиливается на 2% за каждый негативный эффект у цели.";
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
