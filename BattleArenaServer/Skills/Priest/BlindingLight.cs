using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.Priest
{
    public class BlindingLight : Skill
    {
        public BlindingLight()
        {
            name = "BlindingLight";
            coolDown = 4;
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
                        {
                            n.SetDamage(100, "pure");
                            Blind debuffBlind = new Blind(caster.Id, 0, 2);
                            n.HERO.EffectList.Add(debuffBlind);
                            debuffBlind.ApplyEffect(n.HERO);
                        }
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
