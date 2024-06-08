using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;
using System.Xml.Linq;
using System;
using BattleArenaServer.Effects.Debuffs;

namespace BattleArenaServer.Skills.PriestSkills
{
    public class Condemnation : Skill
    {
        int extraDmgPercent = 30;
        public Condemnation()
        {
            name = "Condemnation";
            title = $"Выносит врагу обвинительный приговор, отчего тот получает на {extraDmgPercent}% больше урона.";
            titleUpg = "Враг получает на 50% больше урона";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            range = 2;
            nonTarget = false;
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
                    caster.AP -= requireAP;

                    CondemnationDebuff condemnationDebuff = new CondemnationDebuff(caster.Id, extraDmgPercent, 2);
                    target.EffectList.Add(condemnationDebuff);
                    condemnationDebuff.ApplyEffect(target);

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
                extraDmgPercent = 50;
                return true;
            }
            return false;
        }
    }
}
