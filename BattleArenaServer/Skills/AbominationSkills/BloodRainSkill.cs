﻿using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;
using BattleArenaServer.Services;

namespace BattleArenaServer.Skills.AbominationSkills
{
    public class BloodRainSkill : Skill
    {
        double percent = 0.15;
        public BloodRainSkill()
        {
            name = "Blood Rain";
            title = $"Вызывает кровавый дождь, который накладывает кровотечение на 2 хода на врагов в области.\n" +
                $"Эффект отнимает {Math.Round(percent * 100)}% от разницы максимального ХП врага и заклинателя в конце хода противника.";
            titleUpg = "+1 к радиусу";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            range = 2;
            radius = 1;
            nonTarget = false;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new RangeAoECastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null)
            {
                foreach (var n in UtilityService.GetHexesRadius(requestData.TargetHex, radius))
                {
                    if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                    {
                        int bleedingDmg = Convert.ToInt32((double)(requestData.Caster.MaxHP - n.HERO.MaxHP) * percent);
                        BleedingDebuff bleedingDebuff = new BleedingDebuff(requestData.Caster.Id, bleedingDmg, 3);
                        n.HERO.AddEffect(bleedingDebuff);
                    }
                }

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
                radius += 1;
                stats.radius += 1;
                return true;
            }
            return false;
        }
    }
}
