using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.Priest
{
    public class Smight : Skill
    {
        private int extraDmg = 25;
        public Smight()
        {
            name = "Smight";
            title = "Божественная кара настигает врага, нанося тому чистый урон. Урон увеличивается за каждую способность врага в откате (X). ( 120 + 25 * X )";
            titleUpg = "+20 к доп. урону, +1 к дальности";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 2;
            radius = 0;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(Hero caster, Hero? target, Hex? targetHex)
        {
            if (request.startRequest(caster, target, targetHex, this))
            {
                if (caster != null && target != null)
                {
                    int dmg = 120;
                    foreach (var skill in target.SkillList)
                    {
                        if (skill.coolDownNow > 0)
                            dmg += extraDmg;
                    }

                    caster.AP -= requireAP;
                    AttackService.SetDamage(caster, target, dmg, Consts.DamageType.Pure);
                    coolDownNow = coolDown;
                    return true;
                }
            }
            return false;
        }

        public override bool UpgradeSkill()
        {
            if (!upgraded)
            {
                upgraded = true;
                extraDmg = 45;
                range += 1;
                stats.range += 1;
                return true;
            }
            return false;
        }
    }
}
