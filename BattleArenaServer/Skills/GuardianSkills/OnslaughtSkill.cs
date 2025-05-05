using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.GuardianSkills
{
    public class OnslaughtSkill : Skill
    {
        public OnslaughtSkill()
        {
            name = "Onslaught";
            dmg = 90;
            title = $"Мощный натиск отбрасывает врага назад и наносит {dmg} маг. урона, а Вы становитесь на его место.";
            titleUpg = "-1 к перезарядке, +20 к урону";
            coolDown = 3;
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

            if (requestData.Caster != null && requestData.Target != null && requestData.TargetHex != null && requestData.CasterHex != null)
            {
                Hex? hex = UtilityService.GetOneHexOnDirection(requestData.CasterHex, requestData.TargetHex, 2);
                if (hex != null && hex.IsFree())
                {
                    AttackService.MoveHero(requestData.Target, requestData.TargetHex, hex); // Перемещаем врага
                    AttackService.MoveHero(requestData.Caster, requestData.CasterHex, requestData.TargetHex); // Встаём на его место
                }

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
                dmg += 20;
                coolDown -= 1;
                stats.coolDown -= 1;
                title = $"Мощный натиск отбрасывает врага назад и наносит {dmg} физ. урона, а Вы становитесь на его место.";
                return true;
            }
            return false;
        }
    }
}
