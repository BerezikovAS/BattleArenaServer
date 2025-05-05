using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.GeomantSkills
{
    public class GiantBoulderSkill : Skill
    {
        public GiantBoulderSkill()
        {
            name = "Giant Boulder";
            dmg = 160;
            title = $"Запускает гигантский валун по прямой, который наносит первому врагу на линии {dmg} магического урона и отбрасывает назад." +
                $"\n Если позади врага нет свободной клетки, то он теряет 1 ОД.";
            titleUpg = "+60 к урону, +1 к радиусу полета валуна";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            radius = 2;
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

            if (requestData.Caster != null && requestData.CasterHex != null && requestData.TargetHex != null)
            {
                int pos = 1;
                Hero? target = null;
                Hex? targetHex = null;
                while (pos <= radius && target == null)
                {
                    targetHex = UtilityService.GetOneHexOnDirection(requestData.CasterHex, requestData.TargetHex, pos);
                    target = targetHex?.HERO?.Team != requestData.Caster.Team ? targetHex?.HERO : null;
                    pos++;
                }

                if (target != null && targetHex != null)
                {
                    Hex? hex = UtilityService.GetOneHexOnDirection(requestData.CasterHex, requestData.TargetHex, pos);
                    if (hex != null && hex.IsFree())
                    {
                        AttackService.MoveHero(target, targetHex, hex);
                    }
                    else
                        target.AP -= 1;

                    AttackService.SetDamage(requestData.Caster, target, dmg, dmgType);
                }

                //Если не нашли никого, кому влетит камнем, то это вина кастера)
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
                dmg += 60;
                radius += 1;
                stats.radius += 1;
                title = $"Запускает гигантский валун по прямой, который наносит первому врагу на линии {dmg} магического урона и отбрасывает назад." +
                    $"\n Если позади врага нет свободной клетки, то он теряет 1 ОД.";
                return true;
            }
            return false;
        }
    }
}
