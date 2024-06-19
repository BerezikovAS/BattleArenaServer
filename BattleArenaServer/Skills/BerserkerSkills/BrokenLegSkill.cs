using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.BerserkerSkills
{
    public class BrokenLegSkill : Skill
    {
        int multiply = 2;
        string upgString = "двойной";
        public BrokenLegSkill()
        {
            name = "Broken Leg";
            title = $"Мощная атака, которая обездвиживает противника. Наносит {upgString} урон атаки.";
            titleUpg = "Способность наносит тройной урон от атаки.";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 3;
            nonTarget = false;
            range = 1;
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
                RootDebuff rootDebuff = new RootDebuff(requestData.Caster.Id, 0, 2);
                requestData.Target.AddEffect(rootDebuff);
                rootDebuff.ApplyEffect(requestData.Target);
                requestData.Caster.AP -= requireAP;
                coolDownNow = coolDown;
                AttackService.SetDamage(requestData.Caster, requestData.Target, requestData.Caster.Dmg * multiply, Consts.DamageType.Physical);
                return true;
            }

            return false;
        }

        public override bool UpgradeSkill()
        {
            if (!upgraded)
            {
                upgraded = true;
                multiply = 3;
                upgString = "тройной";
                title = $"Мощная атака, которая обездвиживает противника. Наносит {upgString} урон атаки.";
                return true;
            }
            return false;
        }
    }
}
