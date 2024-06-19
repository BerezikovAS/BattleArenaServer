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

        public override bool Cast(RequestData requestData)
        {
            int alliesCount = 0;
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.Target != null && requestData.TargetHex != null)
            {
                foreach (var n in UtilityService.GetHexesRadius(requestData.TargetHex, radius))
                {
                    if (n.HERO != null && n.HERO.Team == requestData.Caster.Team && n.HERO.Id != requestData.Caster.Id)
                        alliesCount++;
                }
                requestData.Caster.AP -= requireAP;
                coolDownNow = coolDown;
                AttackService.SetDamage(requestData.Caster, requestData.Target, dmg + alliesCount * extraDmg, Consts.DamageType.Physical);
                return true;
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
                title = $"Атакуйте врага единым строем. Урон увеличивается за каждого вашего союзника рядом с целью ({extraDmg} за союзника).";
                return true;
            }
            return false;
        }
    }
}
