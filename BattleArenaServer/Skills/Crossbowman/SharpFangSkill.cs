using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;
using System.Xml.Linq;
using System;
using BattleArenaServer.Services;

namespace BattleArenaServer.Skills.Crossbowman
{
    public class SharpFangSkill : Skill
    {
        int dmg = 125;
        public SharpFangSkill()
        {
            name = "Sharp Fang";
            title = $"Острый шип пронзает врагов на линии, нанося {dmg} чистого урона.";
            titleUpg = "+50 к урону. Дальность полета снаряда неограничена.";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            radius = 4;
            nonTarget = false;
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
                            AttackService.SetDamage(caster, n.HERO, dmg, Consts.DamageType.Pure);
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
                dmg += 50;
                radius += 10;
                stats.radius += 10;
                return true;
            }
            return false;
        }
    }
}
