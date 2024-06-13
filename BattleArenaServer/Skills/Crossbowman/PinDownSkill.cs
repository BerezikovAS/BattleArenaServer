using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;
using System.Xml.Linq;
using System;
using BattleArenaServer.Effects.Debuffs;

namespace BattleArenaServer.Skills.Crossbowman
{
    public class PinDownSkill : Skill
    {
        int dmg = 150;
        public PinDownSkill()
        {
            name = "Pin Down";
            title = $"Массивный болт прибивает врага к земле, отчего тот не может передвигаться. {dmg} физического урона.";
            titleUpg = "+100 к урону.";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
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
                    RootDebuff rootDebuff = new RootDebuff(caster.Id, 0, 2);
                    target.EffectList.Add(rootDebuff);
                    rootDebuff.ApplyEffect(target);
                    caster.AP -= requireAP;
                    coolDownNow = coolDown;
                    AttackService.SetDamage(caster, target, dmg, Consts.DamageType.Physical);
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
                dmg += 100;
                return true;
            }
            return false;
        }
    }
}
