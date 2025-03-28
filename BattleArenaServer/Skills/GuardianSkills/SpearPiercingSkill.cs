﻿using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.GuardianSkills
{
    public class SpearPiercingSkill : Skill
    {
        int armorPiercing = 3;
        public SpearPiercingSkill()
        {
            name = "Spear Piercing";
            dmg = 120;
            title = $"Проведите атаку по врагам по прямой на 2 клетки. Эта атака игнорирует до {armorPiercing} брони и наносит {dmg} физ. урона.";
            titleUpg = "+3 к игнорированию брони, -1 к перезарядке";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            radius = 2;
            nonTarget = false;
            area = Consts.SpellArea.Line;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Physical;
        }

        public new ISkillCastRequest request => new LineCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null && requestData.CasterHex != null)
            {
                requestData.Caster.armorPiercing += ArmorPiercing;
                foreach (var n in UtilityService.GetHexesOneLine(requestData.CasterHex, requestData.TargetHex, radius))
                {
                    if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                        AttackService.SetDamage(requestData.Caster, n.HERO, requestData.Caster.Dmg, dmgType);
                }
                requestData.Caster.armorPiercing -= ArmorPiercing;
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
                armorPiercing += 3;
                coolDown -= 1;
                stats.coolDown -= 1;
                title = $"Проведите атаку по врагам по прямой на 2 клетки. Эта атака игнорирует до {armorPiercing} брони и наносит {dmg} физ. урона.";
                return true;
            }
            return false;
        }

        private int ArmorPiercing(Hero attacker, Hero defender)
        {
            return armorPiercing;
        }
    }
}
