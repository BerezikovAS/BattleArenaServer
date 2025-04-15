using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills._SummonsSkills
{
    public class PeckOutSkill : Skill
    {
        public PeckOutSkill()
        {
            name = "Peck Out";
            dmg = 80;
            title = $"Выклевывает глаза врагу, ослепляет его и наносит {dmg} маг. урона";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
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
                BlindDebuff blindDebuff = new BlindDebuff(requestData.Caster.Id, 0, 2);
                requestData.Target.AddEffect(blindDebuff);

                AttackService.SetDamage(requestData.Caster, requestData.Target, dmg, dmgType);

                requestData.Caster.AP -= requireAP;
                coolDownNow = coolDown;
                return true;
            }

            return false;
        }
    }
}
