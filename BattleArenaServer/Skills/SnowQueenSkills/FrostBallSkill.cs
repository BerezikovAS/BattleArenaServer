using BattleArenaServer.Effects.Unique;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.SnowQueenSkills
{
    public class FrostBallSkill : Skill
    {
        int bounces = 3;
        public FrostBallSkill()
        {
            name = "Frost Ball";
            dmg = 125;
            title = $"Запускает во врага морозный сняряд, который наносит {dmg} маг. урона и в конце хода врага отскакивает в другого ближайшего неприятеля в радиусе 2 клеток. " +
                $"(макс: {bounces} отскока).";
            titleUpg = "+35 к урону";
            coolDown = 5;
            coolDownNow = 0;
            requireAP = 2;
            range = 2;
            nonTarget = false;
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
                FrostBallUnique frostBallUnique = new FrostBallUnique(requestData.Caster.Id, dmg, 99, bounces, requestData.Caster);
                requestData.Target.AddEffect(frostBallUnique);

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
                dmg += 35;
                title = $"Запускает во врага морозный сняряд, который наносит {dmg} маг. урона и в конце хода врага отскакивает в другого ближайшего неприятеля в радиусе 2 клеток. " +
                    $"(макс: {bounces} отскока).";
                return true;
            }
            return false;
        }
    }
}
