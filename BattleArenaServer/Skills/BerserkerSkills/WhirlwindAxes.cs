using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;
using BattleArenaServer.Services;

namespace BattleArenaServer.Skills.BerserkerSkills
{
    public class WhirlwindAxes : Skill
    {
        int dmg = 180;
        public WhirlwindAxes()
        {
            name = "Whirlwind Axes";
            title = "Вихрь топоров атакует всех врагов вокруг, нанося 180 маг. урона";
            titleUpg = "+45 урона, -2 к перезарядке";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            radius = 1;
            range = 0;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new NonTargerAoECastRequest();

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override bool Cast(Hero caster, Hero? target, Hex? targetHex)
        {
            if (request.startRequest(caster, target, targetHex, this))
            {
                if (caster != null && targetHex != null)
                {
                    foreach (var n in UtilityService.GetHexesRadius(targetHex, radius))
                    {
                        if (n.HERO != null && n.HERO.Team != caster.Team)
                            AttackService.SetDamage(caster, n.HERO, dmg, Consts.DamageType.Magic);
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
                coolDown -= 2;
                dmg += 45;
                stats.coolDown -= 2;
                return true;
            }
            return false;
        }
    }
}
