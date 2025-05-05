using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.MusketeerSKills
{
    public class GrenadeSkill : Skill
    {
        int sharpDmg = 100;
        public GrenadeSkill()
        {
            name = "Grenade";
            dmg = 150;
            title = $"Бросает гранату, которая наносит {dmg} маг. урона в месте взрыва и {sharpDmg} урона вокруг.";
            titleUpg = "+30 урона в центре, +15 урона вокруг";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 2;
            radius = 1;
            area = Consts.SpellArea.Radius;
            dmgType = Consts.DamageType.Magic;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new RangeAoECastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null)
            {
                foreach (var n in UtilityService.GetHexesRadius(requestData.TargetHex, radius))
                {
                    if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                    {
                        int dealDmg = n.Distance(requestData.TargetHex) == 0 ? dmg : sharpDmg;
                        AttackService.SetDamage(requestData.Caster, n.HERO, dealDmg, Consts.DamageType.Magic);
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
                dmg += 30;
                sharpDmg += 15;
                title = $"Бросает гранату, которая наносит {dmg} маг. урона в месте взрыва и {sharpDmg} урона вокруг.";
                return true;
            }
            return false;
        }
    }
}
