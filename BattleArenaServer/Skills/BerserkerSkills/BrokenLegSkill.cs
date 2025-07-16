using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.BerserkerSkills
{
    public class BrokenLegSkill : Skill
    {
        public BrokenLegSkill()
        {
            name = "Broken Leg";
            dmg = 160;
            title = $"Мощная атака, которая наносит {dmg} физического урона и  обездвиживает противника.";
            titleUpg = "+60 к урону";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 1;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Physical;
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
                dmg += 60;
                title = $"Мощная атака, которая наносит {dmg} физического урона и  обездвиживает противника.";
                return true;
            }
            return false;
        }
    }
}
