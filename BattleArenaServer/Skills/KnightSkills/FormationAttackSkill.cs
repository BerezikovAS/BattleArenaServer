using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.KnightSkills
{
    public class FormationAttackSkill : Skill
    {
        int dmg = 100;
        int extraDmg = 40;
        public FormationAttackSkill()
        {
            name = "Formation Attack";
            title = $"Атакуйте врага единым строем. Урон увеличивается за каждого вашего союзника рядом с целью ({extraDmg} за союзника).";
            titleUpg = "+10 к урону за союзника. Способность не уходит на перезарядку.";
            coolDown = 1;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 1;
            radius = 1;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(Hero caster, Hero? target, Hex? targetHex)
        {
            int alliesCount = 0;
            if (request.startRequest(caster, target, targetHex, this))
            {
                if (caster != null && target != null && targetHex != null)
                {
                    foreach (var n in UtilityService.GetHexesRadius(targetHex, radius))
                    {
                        if (n.HERO != null && n.HERO.Team == caster.Team && n.HERO.Id != caster.Id)
                            alliesCount++;
                    }
                    caster.AP -= requireAP;
                    coolDownNow = coolDown;
                    AttackService.SetDamage(caster, target, dmg + alliesCount * extraDmg, Consts.DamageType.Physical);
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
                extraDmg += 10;
                coolDown = 0;
                stats.coolDown = 0;
                return true;
            }
            return false;
        }
    }
}
