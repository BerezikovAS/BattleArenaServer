using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.Crossbowman
{
    public class SharpFangSkill : Skill
    {
        public SharpFangSkill()
        {
            name = "Sharp Fang";
            dmg = 135;
            title = $"Острый шип пронзает врагов на линии, нанося {dmg} чистого урона.";
            titleUpg = "+50 к урону. Дальность полета снаряда неограничена.";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            radius = 4;
            nonTarget = false;
            area = Consts.SpellArea.Line;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Pure;
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
                        AttackService.SetDamage(requestData.Caster, n.HERO, dmg, dmgType);
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
                dmg += 50;
                radius += 10;
                stats.radius += 10;
                title = $"Острый шип пронзает врагов на линии, нанося {dmg} чистого урона.";
                return true;
            }
            return false;
        }
    }
}
