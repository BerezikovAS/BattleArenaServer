using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;
using System.Xml.Linq;
using System;
using BattleArenaServer.Effects;

namespace BattleArenaServer.Skills.Priest
{
    public class Restoration : Skill
    {
        public Restoration()
        {
            name = "Restoration";
            coolDown = 5;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 2;
        }

        public ISkillCastRequest request => new AllyTargetCastRequest();

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override bool Cast(List<Hex> _hexes, int _target, int _caster)
        {
            if (request.startRequest(_hexes, _target, _caster, this))
            {
                Hero caster = _hexes[_caster].HERO;
                Hero target = _hexes[_target].HERO;
                if (caster != null & target != null)
                {
                    List<Effect> effects = new List<Effect>();
                    caster.AP -= requireAP;

                    foreach (var effect in target.EffectList)
                    {
                        if (effect.type == "debuff")
                        {
                            effect.RemoveEffect(target);
                            effects.Add(effect);
                        }
                    }
                    foreach (var effect in effects)
                    {
                        target.EffectList.Remove(effect);
                    }
                    target.Heal(200);
                    coolDownNow = coolDown;
                    return true;
                }
            }
            return false;
        }
    }
}
