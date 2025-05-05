using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.FallenKingSkills
{
    public class ParalysisSkill : Skill
    {
        public ParalysisSkill()
        {
            name = "Paralysis";
            dmg = 150;
            title = $"Мёртвый взгляд в глаза парализует врага. Враг получает {dmg} чистого урона и не может действовать в свой ход.";
            titleUpg = "+25 к урону, -1 ОД";
            coolDown = 5;
            coolDownNow = 0;
            requireAP = 4;
            nonTarget = false;
            range = 1;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Pure;
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.Target != null)
            {
                ParalysisDebuff paralysisDebuff = new ParalysisDebuff(requestData.Caster.Id, 0, 2);
                requestData.Target.AddEffect(paralysisDebuff);

                AttackService.SetDamage(requestData.Caster, requestData.Target, dmg, dmgType);

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
                requireAP -= 1;
                dmg += 25;
                title = $"Мёртвый взгляд в глаза парализует врага. Враг получает {dmg} чистого урона и не может действовать в свой ход.";
                return true;
            }
            return false;
        }
    }
}
