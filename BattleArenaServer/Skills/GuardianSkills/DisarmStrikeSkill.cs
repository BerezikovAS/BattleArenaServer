using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.GuardianSkills
{
    public class DisarmStrikeSkill : Skill
    {
        public DisarmStrikeSkill()
        {
            name = "Disarm Strike";
            dmg = 150;
            title = $"Заряженный магический удар наносит {dmg} маг. урона и разоружает врага, отчего тот не может атаковать в свой ход.";
            titleUpg = "+50 к урону";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 1;
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
                DisarmDebuff disarmDebuff = new DisarmDebuff(requestData.Caster.Id, 0, 2);
                requestData.Target.AddEffect(disarmDebuff);

                AttackService.SetDamage(requestData.Caster, requestData.Target, dmg, dmgType);

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
                dmg += 50;
                title = $"Заряженный магический удар наносит {dmg} маг. урона и разоружает врага, отчего тот не может атаковать в свой ход.";
                return true;
            }
            return false;
        }
    }
}
