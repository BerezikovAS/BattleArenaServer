using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;


namespace BattleArenaServer.Skills.Knight
{
    public class FireLine : Skill
    {
        public FireLine()
        {
            name = "Fire Line";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            radius = 3;
        }

        public ISkillCastRequest request => new LineCastRequest();

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
                    foreach (var n in util.GetHexesOneLine(_hexes, _caster, _target, radius))
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
    }
}
