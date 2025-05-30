﻿using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.BerserkerSkills
{
    public class WhirlwindAxesSkill : Skill
    {
        public WhirlwindAxesSkill()
        {
            name = "Whirlwind Axes";
            dmg = 140;
            title = $"Вихрь топоров атакует всех врагов вокруг, нанося {dmg} маг. урона";
            titleUpg = "+35 урона, -2 к перезарядке";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            radius = 1;
            range = 0;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new NonTargerAoECastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null)
            {
                foreach (var n in UtilityService.GetHexesRadius(requestData.TargetHex, radius))
                {
                    if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                        AttackService.SetDamage(requestData.Caster, n.HERO, dmg, dmgType);
                }
                requestData.Caster.SpendAP(requireAP);
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
                coolDown -= 2;
                dmg += 35;
                stats.coolDown -= 2;
                title = $"Вихрь топоров атакует всех врагов вокруг, нанося {dmg} маг. урона";
                return true;
            }
            return false;
        }
    }
}
