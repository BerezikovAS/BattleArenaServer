using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;
using BattleArenaServer.Effects.Debuffs;

namespace BattleArenaServer.Skills.Crossbowman
{
    public class PinDownSkill : Skill
    {
        int dmg = 150;
        public PinDownSkill()
        {
            name = "Pin Down";
            title = $"Массивный болт прибивает врага к земле, отчего тот не может передвигаться. {dmg} физического урона.";
            titleUpg = "+100 к урону.";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
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
                RootDebuff rootDebuff = new RootDebuff(requestData.Caster.Id, 0, 2);
                requestData.Target.AddEffect(rootDebuff);
                rootDebuff.ApplyEffect(requestData.Target);
                requestData.Caster.AP -= requireAP;
                coolDownNow = coolDown;
                AttackService.SetDamage(requestData.Caster, requestData.Target, dmg, Consts.DamageType.Physical);
                return true;
            }

            return false;
        }

        public override bool UpgradeSkill()
        {
            if (!upgraded)
            {
                upgraded = true;
                dmg += 100;
                return true;
            }
            return false;
        }
    }
}
