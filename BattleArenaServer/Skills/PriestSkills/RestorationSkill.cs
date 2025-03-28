using BattleArenaServer.Effects;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.Priest
{
    public class RestorationSkill : Skill
    {
        int heal = 150;
        public RestorationSkill()
        {
            name = "Restoration";
            title = $"Восстанавливает себе или союзнику {heal} ХП.";
            titleUpg = "Также снимает негативные эффекты с цели.";
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

                if (upgraded)
                {
                    foreach (var effect in requestData.Target.EffectList)
                    {
                        if (effect.type == Consts.StatusEffect.Debuff)
                        {
                            effect.RemoveEffect(requestData.Target);
                            effects.Add(effect);
                        }
                    }
                    foreach (var effect in effects)
                    {
                        requestData.Target.EffectList.Remove(effect);
                    }
                }

                requestData.Target.Heal(heal);

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
                title = $"Восстанавливает себе или союзнику {heal} ХП и снимает негативные эффекты.";
                return true;
            }
            return false;
        }
    }
}
