using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.LizardmanSkills
{
    public class SwiftFeintSkill : Skill
    {
        public SwiftFeintSkill()
        {
            name = "Swift Feint";
            dmg = 145;
            title = $"Наносит врагу {dmg} маг. урона, замедляет его и меняется с ним местами.";
            titleUpg = "+40 к урону";
            coolDown = 3;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 1;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (!request.startRequest(requestData, this, gameData))
                return false;

            if (requestData.Caster != null && requestData.Target != null && requestData.TargetHex != null && requestData.CasterHex != null)
            {
                SlowDebuff slowDebuff = new SlowDebuff(requestData.Caster.Id, 0, 2);
                requestData.Target.AddEffect(slowDebuff, gameData);

                requestData.CasterHex.SetHero(requestData.Target);
                requestData.TargetHex.SetHero(requestData.Caster);

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
                dmg += 40;
                title = $"Наносит врагу {dmg} маг. урона, замедляет его и меняется с ним местами.";
                return true;
            }
            return false;
        }
    }
}
