using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.AssassinSkills
{
    public class RuptureSkill : Skill
    {
        int ruptureDmg = 45;
        int ruptureDuration = 2;
        public RuptureSkill()
        {
            dmg = 150;
            name = "Rupture";
            title = $"Наносит {dmg} магического урона и подрезает сухожилие жертвы. Передвигаясь, цель будет терять {ruptureDmg} ХП за пройденный гекс.";
            titleUpg = "Негативный эффект длится 2 хода.";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 1;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.Target != null)
            {
                RuptureDebuff ruptureDebuff = new RuptureDebuff(requestData.Caster.Id, ruptureDmg, ruptureDuration);
                requestData.Target.AddEffect(ruptureDebuff);

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
                ruptureDuration += 1;
                return true;
            }
            return false;
        }
    }
}
