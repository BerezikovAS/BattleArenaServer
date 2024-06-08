using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;


namespace BattleArenaServer.Skills.Knight
{
    public class FireLine : Skill
    {
        int dmg = 200;
        public FireLine()
        {
            name = "Fire Line";
            title = "Поджигает всех врагов по линии перед собой, нанося 200 маг. урона";
            titleUpg = "+75 урона, +1 к дальности";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            radius = 3;
            area = Consts.SpellArea.Line;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new LineCastRequest();

        public override bool Cast(Hero caster, Hero? target, Hex? targetHex)
        {
            if (request.startRequest(caster, target, targetHex, this))
            {
                if (caster != null && targetHex != null)
                {
                    foreach (var n in UtilityService.GetHexesOneLine(caster, targetHex, radius))
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
                dmg += 75;
                radius += 1;
                stats.radius += 1;
                return true;
            }
            return false;
        }
    }
}
