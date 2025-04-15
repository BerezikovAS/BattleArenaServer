using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.SeraphimSkills
{
    public class BattleRushSkill : Skill
    {
        public BattleRushSkill()
        {
            name = "Battle Rush";
            dmg = 120;
            title = $"Проносится вперед и занимает самую последнюю свободную клетку. В месте остановки наносит {dmg} маг. урона врагам вокруг.";
            titleUpg = "+40 к урону";
            coolDown = 3;
            coolDownNow = 0;
            requireAP = 1;
            radius = 3;
            range = 3;
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
                Hex? targetHex = null;
                Hex? endHex = null;
                while (pos <= radius)
                {
                    targetHex = UtilityService.GetOneHexOnDirection(requestData.CasterHex, requestData.TargetHex, pos);
                    if (targetHex != null && targetHex.IsFree())
                        endHex = targetHex;
                    pos++;
                }

                if (endHex == null)
                    endHex = requestData.CasterHex;

                AttackService.MoveHero(requestData.Caster, requestData.CasterHex, endHex);

                foreach (var hex in UtilityService.GetHexesRadius(endHex, 1))
                {
                    if (hex != null && hex.HERO != null && hex.HERO.Team != requestData.Caster.Team)
                        AttackService.SetDamage(requestData.Caster, hex.HERO, dmg, dmgType);
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
                dmg += 40;
                title = $"Проносится вперед и занимает самую последнюю свободную клетку. В месте остановки наносит {dmg} маг. урона врагам вокруг.";
                return true;
            }
            return false;
        }
    }
}
