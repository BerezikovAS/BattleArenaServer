using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;
using System.Xml.Linq;

namespace BattleArenaServer.Skills.Knight
{
    public class HailStrike : Skill
    {
        public HailStrike()
        {
            name = "Hail Strike";
            coolDown = 5;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 2;
            radius = 1;
        }

        public ISkillCastRequest request => new RangeAoECastRequest();

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override bool Cast(List<Hex> _hexes, int _target, int _caster)
        {
            if (request.startRequest(_hexes, _target, _caster, this))
            {
                Hero caster = _hexes[_caster].HERO;
                UtilityService util = new UtilityService();

                if (caster != null)
                {
                    foreach (var n in util.GetHexesRadius(_hexes, _target, radius))
                    {
                        if (n.HERO != null && n.HERO.Team != caster.Team)
                            n.SetDamage(200, "magic");
                    }
                    caster.AP -= requireAP;
                    coolDownNow = coolDown;
                    return true;
                }
            }
            return false;
        }        

        public void UpgradeSkill()
        {
            if (!upgraded)
            {
                upgraded = true;
                range += 1;
                radius += 1;
            }
        }
    }
}
