using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.Priest
{
    public class SmightSkill : Skill
    {
        int extraDmg = 25;
        public SmightSkill()
        {
            name = "Smight";
            dmg = 100;
            title = $"Божественная кара настигает врага, нанося тому чистый урон. Урон увеличивается за каждую способность врага в откате (X). ( {dmg} + {extraDmg} * X )";
            titleUpg = "+20 к доп. урону, +1 к дальности";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 2;
            radius = 0;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Pure;
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.Target != null)
            {
                int totalDmg = dmg;
                foreach (var skill in requestData.Target.SkillList)
                {
                    if (skill.coolDownNow > 0)
                        totalDmg += extraDmg;
                }

                requestData.Caster.AP -= requireAP;
                AttackService.SetDamage(requestData.Caster, requestData.Target, totalDmg, dmgType);
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
                extraDmg += 20;
                range += 1;
                stats.range += 1;
                title = $"Божественная кара настигает врага, нанося тому чистый урон. Урон увеличивается за каждую способность врага в откате (X). ( {dmg} + {extraDmg} * X )";
                return true;
            }
            return false;
        }
    }
}
