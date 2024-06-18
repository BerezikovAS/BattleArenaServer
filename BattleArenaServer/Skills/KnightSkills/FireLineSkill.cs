using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;


namespace BattleArenaServer.Skills.Knight
{
    public class FireLineSkill : Skill
    {
        int dmg = 200;
        public FireLineSkill()
        {
            name = "Fire Line";
            title = "Поджигает всех врагов по линии перед собой, нанося 200 маг. урона";
            titleUpg = "+75 урона, +1 к дальности";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            radius = 3;
            area = Consts.SpellArea.Line;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new LineCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null)
            {
                foreach (var n in UtilityService.GetHexesOneLine(requestData.Caster, requestData.TargetHex, radius))
                {
                    if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                        AttackService.SetDamage(requestData.Caster, n.HERO, dmg, Consts.DamageType.Magic);
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
                dmg += 75;
                radius += 1;
                stats.radius += 1;
                return true;
            }
            return false;
        }
    }
}
