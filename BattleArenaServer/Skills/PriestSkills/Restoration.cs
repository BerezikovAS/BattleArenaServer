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
            title = "Восстанавливает себе или союзнику 200 ХП и снимает негативные эффекты";
            titleUpg = "-1 к стоимости в ОД, +1 к дальности";
            coolDown = 5;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 2;
            area = Consts.SpellArea.AllyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new AllyTargetCastRequest();

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override bool Cast(Hero caster, Hero? target, Hex? targetHex)
        {
            if (request.startRequest(caster, target, targetHex, this))
            {
                if (caster != null && target != null)
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

        public override bool UpgradeSkill()
        {
            if (!upgraded)
            {
                upgraded = true;
                requireAP -= 1;
                stats.requireAP -= 1;
                range += 1;
                stats.range += 1;
                return true;
            }
            return false;
        }
    }
}
