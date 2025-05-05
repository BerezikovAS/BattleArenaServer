using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.PlagueDoctorSkills
{
    public class AcidSplashSkill : Skill
    {
        public AcidSplashSkill()
        {
            name = "Acid Splash";
            dmg = 125;
            title = $"Выплескивает едкую кислоту на врагов, которая наносит {dmg} урона. Тип урона определяется по меньшему параметру брони и сопротивления.";
            titleUpg = "+20 к урону";
            coolDown = 3;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 2;
            radius = 2;
            area = Consts.SpellArea.SmallConus;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new LineCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null && requestData.CasterHex != null)
            {
                AttackService.dealedDmg = 0;
                foreach (var n in UtilityService.GetHexesSmallCone(requestData.CasterHex, requestData.TargetHex, radius))
                {
                    if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                    {
                        Consts.DamageType damageType = n.HERO.Armor < n.HERO.Resist ? Consts.DamageType.Physical : Consts.DamageType.Magic;
                        AttackService.SetDamage(requestData.Caster, n.HERO, dmg, damageType);
                    }
                }

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
                dmg += 20;
                title = $"Выплескивает едкую кислоту на врагов, которая наносит {dmg} урона. Тип урона определяется по меньшему параметру брони и сопротивления.";
                return true;
            }
            return false;
        }
    }
}
