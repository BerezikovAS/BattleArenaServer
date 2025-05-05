using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.NecromancerSkills
{
    public class GraveHandsSkill : Skill
    {
        public GraveHandsSkill()
        {
            name = "Grave Hands";
            dmg = 145;
            title = $"Руки мертвецов пробиваются из-под земли и хватают врагов на пути. Все враги получают {dmg} магического урона и замедляются.";
            titleUpg = "+1 к дальности, -1 к перезарядке";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            radius = 3;
            nonTarget = false;
            area = Consts.SpellArea.Line;
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
                foreach (var n in UtilityService.GetHexesOneLine(requestData.CasterHex, requestData.TargetHex, radius))
                {
                    if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                    {
                        SlowDebuff slowDebuff = new SlowDebuff(requestData.Caster.Id, 0, 2);
                        n.HERO.AddEffect(slowDebuff);

                        AttackService.SetDamage(requestData.Caster, n.HERO, dmg, dmgType);
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
                coolDown -= 1;
                stats.coolDown -= 1;
                radius += 1;
                stats.radius += 1;
                title = $"Руки мертвецов пробиваются из-под земли и хватают врагов на пути. Все враги получают {dmg} магического урона и замедляются.";
                return true;
            }
            return false;
        }
    }
}
