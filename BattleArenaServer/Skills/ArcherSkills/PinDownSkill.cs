using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.Crossbowman
{
    public class PinDownSkill : Skill
    {
        public PinDownSkill()
        {
            name = "Pin Down";
            dmg = 150;
            title = $"Массивный болт прибивает врага к земле, отчего тот не может передвигаться. {dmg} магического урона.";
            titleUpg = "+100 к урону.";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            range = 2;
            nonTarget = false;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (!request.startRequest(requestData, this, gameData))
                return false;

            if (requestData.Caster != null && requestData.Target != null)
            {
                RootDebuff rootDebuff = new RootDebuff(requestData.Caster.Id, 0, 2);
                requestData.Target.AddEffect(rootDebuff, gameData);
                
                AttackService.SetDamage(requestData.Caster, requestData.Target, dmg, dmgType, gameData);
                
                requestData.Caster.SpendAP(requireAP, gameData);
                coolDownNow = coolDown;
                return true;
            }

            return false;
        }

        public override bool UpgradeSkill(GameData gameData)
        {
            if (!upgraded)
            {
                upgraded = true;
                dmg += 100;
                title = $"Массивный болт прибивает врага к земле, отчего тот не может передвигаться. {dmg} физического урона.";
                return true;
            }
            return false;
        }
    }
}
