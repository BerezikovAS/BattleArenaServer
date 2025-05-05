using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.InvokerSkills
{
    public class ArcaneBoltSkill : Skill
    {
        public ArcaneBoltSkill()
        {
            name = "Arcane Bolt";
            dmg = 160;
            title = $"Магический снаряд наносит врагу {dmg} маг. урона.";
            titleUpg = "+40 к урону";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 3;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.Target != null)
            {
                AttackService.SetDamage(requestData.Caster, requestData.Target, dmg, dmgType);

                requestData.Caster.SpendAP(requireAP);
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
                dmg += 40;
                title = $"Магический снаряд наносит врагу {dmg} маг. урона.";
                return true;
            }
            return false;
        }
    }
}
