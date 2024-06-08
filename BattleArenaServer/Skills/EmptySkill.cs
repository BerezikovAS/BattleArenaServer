using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills
{
    public class EmptySkill : Skill
    {
        public EmptySkill()
        {
            name = "-None-";
            coolDown = 100;
            coolDownNow = 0;
            requireAP = 0;
            nonTarget = true;
            stats = new SkillStats(coolDown, requireAP, 0, 0);
        }

        public new ISkillCastRequest request => new NontargetCastRequest();

        public override bool Cast(Hero caster, Hero? target, Hex? targetHex)
        {
            return false;
        }

        public override bool UpgradeSkill()
        {
            throw new NotImplementedException();
        }
    }
}
