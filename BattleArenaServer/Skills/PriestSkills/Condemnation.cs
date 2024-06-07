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
        int defence = 30;
        public Condemnation()
        {
            name = "Condemnation";
            title = $"Выносит врагу обвинительный приговор, отчего тот получает на 30% больше урона.";
            titleUpg = "+1 к броне, +1 к сопротивлению, +1 к дальности";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            range = 2;
            nonTarget = false;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override bool Cast(Hero caster, Hero? target, Hex? targetHex)
        {
            if (request.startRequest(caster, target, targetHex, this))
            {
                if (caster != null && target != null)
                {
                    caster.AP -= requireAP;

                    CondemnationDebuff condemnationDebuff = new CondemnationDebuff(caster.Id, defence, 2);
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
                defence += 1;
                range += 1;
                stats.range += 1;
                return true;
            }
            return false;
        }
    }
}
