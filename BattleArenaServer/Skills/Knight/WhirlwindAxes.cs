using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;
using System.Xml.Linq;
using System;
using BattleArenaServer.Services;

namespace BattleArenaServer.Skills.Knight
{
    public class WhirlwindAxes : Skill
    {
        public WhirlwindAxes()
        {
            name = "Whirlwind Axes";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            radius = 1;
        }

        public ISkillCastRequest request => new NonTargerAoECastRequest();

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
                    foreach (var n in util.GetHexesRadius(_hexes, _caster, radius))
                    {
                        if (n.HERO != null && n.HERO.Team != caster.Team)
                            n.SetDamage(180, "magic");
                    }
                    caster.AP -= requireAP;
                    coolDownNow = coolDown;
                    return true;
                }
            }
            return false;
        }
    }
}
