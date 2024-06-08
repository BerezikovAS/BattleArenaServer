using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;
using System;
using static BattleArenaServer.Models.Consts;

namespace BattleArenaServer.Skills.Knight
{
    public class ShieldBash : Skill
    {
        int dmg = 60;
        int extraDmg = 20;
        int loseAP = 1;
        public ShieldBash()
        {
            name = "Shieldbash";
            title = "Оглушает врага и наносит ему физический урон, зависящий от брони владельца";
            titleUpg = "+75 к лечению, +1 к доп. броне";
            coolDown = 4;
            coolDownNow = 0;
            range = 1;
            requireAP = 2;
            nonTarget = false;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(Hero caster, Hero? target, Hex? targetHex)
        {
            if (request.startRequest(caster, target, targetHex, this))
            {
                if (target != null && caster != null)
                {
                    caster.AP -= requireAP;
                    target.AP -= loseAP;
                    AttackService.SetDamage(caster, target, dmg + extraDmg * caster.Armor, DamageType.Physical);
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
                loseAP += 1;
                extraDmg += 10;
                return true;
            }
            return false;
        }
    }
}
