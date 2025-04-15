using BattleArenaServer.Effects;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills._CommonSkills
{
    public class MoveSkill : Skill
    {
        public MoveSkill()
        {
            name = "Move";
            title = $"Перемещение на соседний гекс.";
            titleUpg = "";
            coolDown = 0;
            coolDownNow = 0;
            requireAP = 1;
            nonTarget = false;
            range = 1;
            area = Consts.SpellArea.NonTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new MoveCastRequest();

        public override bool Cast(RequestData requestData)
        {
            bool isHaste = false;

            Effect? slow = requestData.Caster.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Slow));
            if (slow != null)
                requireAP = 2;

            Effect? haste = requestData.Caster.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Haste));
            if (haste != null)
            {
                isHaste = true;
                requestData.Caster.EffectList.Remove(haste);
                requireAP = 0;
            }

            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.CasterHex != null && requestData.TargetHex != null)
            {
                if (!isHaste)
                    requestData.Caster.AP -= requireAP;

                requireAP = 1;
                AttackService.MoveHero(requestData.Caster, requestData.CasterHex, requestData.TargetHex);
                return true;
            }
            requireAP = 1;
            return false;
        }

        public override bool UpgradeSkill()
        {
            return false;
        }
    }
}
