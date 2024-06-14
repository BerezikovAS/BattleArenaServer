using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;
using BattleArenaServer.Effects;

namespace BattleArenaServer.Skills.Priest
{
    public class RestorationSkill : Skill
    {
        public RestorationSkill()
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

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.Target != null)
            {
                List<Effect> effects = new List<Effect>();

                foreach (var effect in requestData.Target.EffectList)
                {
                    if (effect.type == "debuff")
                    {
                        effect.RemoveEffect(requestData.Target);
                        effects.Add(effect);
                    }
                }
                foreach (var effect in effects)
                {
                    requestData.Target.EffectList.Remove(effect);
                }

                requestData.Target.Heal(200);

                requestData.Caster.AP -= requireAP;
                coolDownNow = coolDown;
                return true;
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
