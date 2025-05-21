using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.SnowQueenSkills
{
    public class IceShardSkill : Skill
    {
        int percentDmg = 50;
        public IceShardSkill()
        {
            name = "Ice Shard";
            dmg = 130;
            title = $"На выбранного врага обрушивается ледянной шип, который наносит {dmg} маг. урона. Враги по соседству также получают {percentDmg}% этого урона.";
            titleUpg = "+40 к урону";
            coolDown = 3;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 3;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.Target != null && requestData.TargetHex != null)
            {
                AttackService.SetDamage(requestData.Caster, requestData.Target, dmg, dmgType);

                foreach (var hex in UtilityService.GetHexesRadius(requestData.TargetHex, 1))
                {
                    if (hex.HERO != null && hex.HERO.Id != requestData.Target.Id && hex.HERO.Team != requestData.Caster.Team)
                        AttackService.SetDamage(requestData.Caster, hex.HERO, (int)(Convert.ToDouble(dmg * percentDmg) / 100), dmgType);
                }

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
                dmg += 40;
                title = $"На выбранного врага обрушивается ледянной шип, который наносит {dmg} маг. урона. Враги по соседству также получают {percentDmg}% этого урона.";
                return true;
            }
            return false;
        }
    }
}
