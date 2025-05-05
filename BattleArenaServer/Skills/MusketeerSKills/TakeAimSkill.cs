using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.MusketeerSKills
{
    public class TakeAimSkill : Skill
    {
        int percentDmg = 55;
        public TakeAimSkill()
        {
            name = "Take Aim";
            title = $"Тратит все свои ОД. За каждое потраченное ОД наносит физический урон равный {percentDmg}% от значения атаки.";
            titleUpg = "+10% к урону";
            coolDown = 2;
            coolDownNow = 0;
            requireAP = 1;
            nonTarget = false;
            range = 3;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Physical;
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.Target != null)
            {
                int availableAP = requestData.Caster.AP;
                int dealDmg = (int)(Convert.ToDouble(percentDmg * availableAP * requestData.Caster.Dmg) / 100);

                AttackService.SetDamage(requestData.Caster, requestData.Target, dealDmg, dmgType);

                requestData.Caster.SpendAP(availableAP);
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
                percentDmg += 10;
                title = $"Тратит все свои ОД. За каждое потраченное ОД наносит физический урон равный {percentDmg}% от значения атаки.";
                return true;
            }
            return false;
        }
    }
}
