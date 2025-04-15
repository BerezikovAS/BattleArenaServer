using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.DruidSkills
{
    public class NatureBalanceSkill : Skill
    {
        int heal = 100;
        public NatureBalanceSkill()
        {
            name = "Nature Balance";
            title = $"Вы и выбранная цель уравниваете свои ХП в процентном соотношении. Если целью был союзник, то дополнительно восстановливает обоим {heal} ХП.";
            titleUpg = "-1 к перезарядке и к затратам ОД, +1 к дальности";
            coolDown = 5;
            coolDownNow = 0;
            requireAP = 2;
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

                if (!request.startRequest(requestData, this) || requestData.Target.IsMainHero == false)
                    return false;

                double totalHP = requestData.Target.HP + requestData.Caster.HP;
                double totalMaxHP = requestData.Target.MaxHP + requestData.Caster.MaxHP;
                double percent = totalHP / totalMaxHP;

                requestData.Caster.HP = (int)(Convert.ToDouble(requestData.Caster.MaxHP) * percent);
                requestData.Target.HP = (int)(Convert.ToDouble(requestData.Target.MaxHP) * percent);

                if (requestData.Target.Team == requestData.Caster.Team)
                {
                    requestData.Caster.Heal(heal);
                    requestData.Target.Heal(heal);
                }

                requestData.Caster.AP -= requireAP;
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
                stats.range += 1;
                coolDown -= 1;
                stats.coolDown -= 1;
                requireAP -= 1;
                title = $"Вы и выбранная цель уравниваете свои ХП в процентном соотношении. Если целью был союзник, то дополнительно восстановливает обоим {heal} ХП.";
                return true;
            }
            return false;
        }
    }
}
