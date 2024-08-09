using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.KnightSkills
{
    public class RetributionSkill : Skill
    {
        int dmgPercent = 1;
        public RetributionSkill()
        {
            name = "Retribution";
            dmg = 100;
            title = $"Мощная размашистая атака, сила которой зависит от процента Вашего недостающего здоровья. ({dmg} + {dmgPercent}% за каждый 1% потерянного здоровья)";
            titleUpg = "+1% к урону за недостающее здоровье.";
            coolDown = 2;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 1;
            radius = 1;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Physical;
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            int alliesCount = 0;
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.Target != null && requestData.TargetHex != null)
            {
                foreach (var n in UtilityService.GetHexesRadius(requestData.TargetHex, radius))
                {
                    if (n.HERO != null && n.HERO.Team == requestData.Caster.Team && n.HERO.Id != requestData.Caster.Id && n.HERO.type != Consts.HeroType.Obstacle)
                        alliesCount++;
                }
                requestData.Caster.AP -= requireAP;
                coolDownNow = coolDown;
                AttackService.SetDamage(requestData.Caster, requestData.Target, dmg + alliesCount * extraDmg, dmgType);
                return true;
            }

            return false;
        }

        public override bool UpgradeSkill()
        {
            if (!upgraded)
            {
                upgraded = true;
                dmgPercent += 1;
                title = $"Мощная размашистая атака, сила которой зависит от процента Вашего недостающего здоровья. ({dmg} + {dmgPercent}% за каждый 1% потерянного здоровья)";
                return true;
            }
            return false;
        }
    }
}
