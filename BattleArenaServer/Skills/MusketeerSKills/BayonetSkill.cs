using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.MusketeerSKills
{
    public class BayonetSkill : Skill
    {
        public BayonetSkill()
        {
            name = "Bayonet";
            dmg = 100;
            title = $"Отталкивает врага от себя штык-ножом и наносит тому {dmg} маг. урона.";
            titleUpg = "-1 к перезарядке, +20 к урону";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            radius = 1;
            range = 1;
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

            if (requestData.Caster != null && requestData.Target != null && requestData.CasterHex != null && requestData.TargetHex != null)
            {
                Hex? hex = UtilityService.GetOneHexOnDirection(requestData.CasterHex, requestData.TargetHex, 2);
                if (hex != null && hex.IsFree())
                {
                    AttackService.MoveHero(requestData.Target, requestData.TargetHex, hex);
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
                title = $"Отталкивает врага от себя штык-ножом и наносит тому {dmg} маг. урона.";
                return true;
            }
            return false;
        }
    }
}
