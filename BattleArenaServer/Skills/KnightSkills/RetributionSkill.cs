using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.KnightSkills
{
    public class RetributionSkill : Skill
    {
        int extraDmg = 1;
        public RetributionSkill()
        {
            name = "Retribution";
            dmg = 110;
            title = $"Мощная размашистая атака, сила которой зависит от процента Вашего недостающего здоровья. ({dmg} + {extraDmg} за каждый 1% потерянного здоровья)";
            titleUpg = "+1 к урону за 1% недостающего здоровья.";
            coolDown = 2;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 1;
            radius = 1;
            area = Consts.SpellArea.Conus;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Physical;
        }

        public new ISkillCastRequest request => new LineCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null && requestData.CasterHex != null)
            {
                double lostHealthPercent = (requestData.Caster.HP / requestData.Caster.MaxHP);
                foreach (var n in UtilityService.GetHexesCone(requestData.CasterHex, requestData.TargetHex, radius))
                {
                    if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                        AttackService.SetDamage(requestData.Caster, n.HERO, dmg + Convert.ToInt32(lostHealthPercent) * extraDmg, dmgType);
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
                extraDmg += 1;
                title = $"Мощная размашистая атака, сила которой зависит от процента Вашего недостающего здоровья. ({dmg} + {extraDmg} за каждый 1% потерянного здоровья)";
                return true;
            }
            return false;
        }
    }
}
