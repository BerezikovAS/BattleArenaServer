using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.BerserkerSkills
{
    public class BrokenLegSkill : Skill
    {
        int multiply = 2;
        public BrokenLegSkill()
        {
            name = "Broken Leg";
            title = "Мощная атака, которая обездвиживает противника. Наносит двойной урон атаки.";
            titleUpg = "Способность наносит тройной урон от атаки.";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 3;
            nonTarget = false;
            range = 1;
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
                    AttackService.SetDamage(caster, target, caster.Dmg * multiply, Consts.DamageType.Physical);
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
                multiply = 3;
                return true;
            }
            return false;
        }
    }
}
