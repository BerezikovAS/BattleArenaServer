using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;
using System.Xml.Linq;

namespace BattleArenaServer.Skills.Knight
{
    public class HailStrike : Skill
    {
        public HailStrike()
        {
            name = "Hail Strike";
            title = "Обрушивает мощный град на врагов в области, нанося 200 маг. урона каждому";
            titleUpg = "+1 к радиусу, +1 к дальности";
            coolDown = 0; //5;
            coolDownNow = 0;
            requireAP = 1; //2;
            nonTarget = false;
            range = 2;
            radius = 1;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new RangeAoECastRequest();

        public override bool Cast(Hero caster, Hero? target, Hex? targetHex)
        {
            if (request.startRequest(caster, target, targetHex, this))
            {
                if (caster != null && targetHex != null)
                {
                    foreach (var n in UtilityService.GetHexesRadius(targetHex, radius))
                    {
                        if (n.HERO != null && n.HERO.Team != caster.Team)
                            AttackService.SetDamage(caster, n.HERO, 200, Consts.DamageType.Magic);
                    }
                    caster.AP -= requireAP;
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
                range += 1;
                radius += 1;
                stats.range += 1;
                stats.radius += 1;
                return true;
            }
            return false;
        }
    }
}
