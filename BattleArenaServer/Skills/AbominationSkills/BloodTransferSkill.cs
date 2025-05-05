using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;
using BattleArenaServer.Services;

namespace BattleArenaServer.Skills.AbominationSkills
{
    public class BloodTransferSkill : Skill
    {
        public BloodTransferSkill()
        {
            name = "Blood Transfer";
            title = $"Вы теряете 10% максимального запаса ХП. Союзник или Вы восстанавливаете это кол-во ХП, а враг получает магический урон в таком объеме.";
            titleUpg = "-2 к перезарядке. +1 к дальности";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            range = 2;
            nonTarget = false;
            dmgType = Consts.DamageType.Magic;
            area = Consts.SpellArea.HeroTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public override bool Cast(RequestData requestData)
        {
            if (requestData.Target != null && requestData.Caster != null)
            {
                if (requestData.Target.Team != requestData.Caster.Team)
                    request = new EnemyTargetCastRequest();
                else
                    request = new AllyTargetCastRequest();

                if (!request.startRequest(requestData, this))
                    return false;

                int healDmg = Convert.ToInt32((double)requestData.Caster.MaxHP * 0.1);

                requestData.Caster.MaxHP -= healDmg;
                if (requestData.Caster.HP > requestData.Caster.MaxHP)
                    requestData.Caster.HP = requestData.Caster.MaxHP;

                if (requestData.Target.Team != requestData.Caster.Team)
                    AttackService.SetDamage(requestData.Caster, requestData.Target, healDmg, dmgType);
                else
                    requestData.Target.Heal(healDmg);

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
                range += 1;
                coolDown -= 2;
                stats.range += 1;
                stats.coolDown -= 2;
                title = $"Вы теряете 10% максимального запаса ХП. Союзник восстанавливает это кол-во ХП, а враг получает магический урон в таком объеме.";
                return true;
            }
            return false;
        }
    }
}
