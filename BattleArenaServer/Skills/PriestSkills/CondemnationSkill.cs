using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;
using BattleArenaServer.Effects.Debuffs;

namespace BattleArenaServer.Skills.PriestSkills
{
    public class CondemnationSkill : Skill
    {
        int extraDmgPercent = 30;
        public CondemnationSkill()
        {
            name = "Condemnation";
            title = $"Выносит врагу обвинительный приговор, отчего тот получает на {extraDmgPercent}% больше урона.";
            titleUpg = "Враг получает на 50% больше урона";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            range = 2;
            nonTarget = false;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.Target != null)
            {
                CondemnationDebuff condemnationDebuff = new CondemnationDebuff(requestData.Caster.Id, extraDmgPercent, 2);
                requestData.Target.EffectList.Add(condemnationDebuff);
                condemnationDebuff.ApplyEffect(requestData.Target);

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
                extraDmgPercent = 50;
                return true;
            }
            return false;
        }
    }
}
