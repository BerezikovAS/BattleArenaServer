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
        }

        public ISkillCastRequest request => new NontargetCastRequest();

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override bool Cast(List<Hex> _hexes, int _target, int _caster)
        {
            return false;
        }
    }
}
