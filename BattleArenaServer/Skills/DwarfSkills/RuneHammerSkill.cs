using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.DwarfSkills
{
    public class RuneHammerSkill : Skill
    {
        public RuneHammerSkill()
        {
            name = "Rune Hammer";
            dmg = 60;
            title = $"Бьёт зачарованным молотом по врагу, отчего тот получает {dmg} физического, {dmg} магического и {dmg} чистого урона.";
            titleUpg = "+15 ко всем типам урона";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
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
                AttackService.SetDamage(requestData.Caster, requestData.Target, dmg, Consts.DamageType.Physical);
                if (requestData.Caster.HP > 0 && requestData.Target.HP > 0) // Проверяем не сдох ли враг или мы сами (такое тоже возможно)
                    AttackService.SetDamage(requestData.Caster, requestData.Target, dmg, Consts.DamageType.Magic);
                if (requestData.Caster.HP > 0 && requestData.Target.HP > 0) // Проверяем не сдох ли враг или мы сами (такое тоже возможно)
                    AttackService.SetDamage(requestData.Caster, requestData.Target, dmg, Consts.DamageType.Pure);

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
                dmg += 15;
                title = $"Бьёт зачарованным молотом по врагу, отчего тот получает {dmg} физического, {dmg} магического и {dmg} чистого урона.";
                return true;
            }
            return false;
        }
    }
}
