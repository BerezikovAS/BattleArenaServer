using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.CultistSkills
{
    public class DevourSkill : Skill
    {
        double percentHP = 20;
        public DevourSkill()
        {
            name = "Devour";
            dmg = 0;
            title = $"Пожирает врага менее чем с {(int)percentHP}% ХП, не оставляя ему никаких шансов.";
            upgraded = true;
            coolDown = 5;
            coolDownNow = 0;
            requireAP = 2;
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

            if (requestData.Caster != null && requestData.Target != null && requestData.TargetHex != null)
            {
                double targetHp = Convert.ToDouble(requestData.Target.HP);
                double targetMaxHp = Convert.ToDouble(requestData.Target.MaxHP);

                if ((targetHp / targetMaxHp) > (percentHP / 100))
                    return false;

                //Съедаем бедолагу
                AttackService.KillHero(requestData.Target);

                requestData.Caster.SpendAP(requireAP);
                coolDownNow = coolDown;
                return true;
            }

            return false;
        }

        public override bool UpgradeSkill()
        {
            return false;
        }
    }
}
