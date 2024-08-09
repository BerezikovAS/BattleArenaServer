using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.ChaosSkills
{
    public class HellJawsSkill : Skill
    {
        int extraDmg = 40;
        public HellJawsSkill()
        {
            name = "Hell Jaws";
            dmg = 120;
            title = $"Враг получает {dmg} физичского урона. Урон увеличивается на {extraDmg} за каждого героя или объект вокруг цели.";
            titleUpg = $"-1 к ОД, +1 к дальности.";
            coolDown = 3;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 2;
            radius = 1;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Physical;
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.Target != null && requestData.TargetHex != null)
            {
                int heroesCnt = 0;

                foreach (var n in UtilityService.GetHexesRadius(requestData.TargetHex, radius))
                {
                    if ((n.HERO != null && n.HERO.Id != requestData.Target.Id) || n.OBSTACLE != null)
                        heroesCnt++;
                }

                AttackService.SetDamage(requestData.Caster, requestData.Target, dmg + extraDmg * heroesCnt, dmgType);
                requestData.Caster.AP -= requireAP;
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
                range += 1;
                stats.range += 1;
                requireAP -= 1;
                return true;
            }
            return false;
        }
    }
}
