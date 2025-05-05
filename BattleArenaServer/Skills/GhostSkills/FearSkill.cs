using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.GhostSkills
{
    public class FearSkill : Skill
    {
        public FearSkill()
        {
            name = "Fear";
            dmg = 125;
            title = $"Издает устрашающий вопль, который пугает врагов и наносит им {dmg} маг. урона. Испуганные враги могут двигаться только в направлении от Вас.";
            titleUpg = "+25 урона, -1 к затратам ОД";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            radius = 2;
            range = 0;
            area = Consts.SpellArea.Radius;
            dmgType = Consts.DamageType.Magic;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new NonTargerAoECastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (request.startRequest(requestData, this))
            {
                if (requestData.Caster != null && requestData.CasterHex != null)
                {
                    foreach (var n in UtilityService.GetHexesRadius(requestData.CasterHex, radius))
                    {
                        if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                        {
                            FearDebuff fearDebuff = new FearDebuff(requestData.Caster.Id, 0, 2, requestData.Caster.Name);
                            n.HERO.AddEffect(fearDebuff);

                            AttackService.SetDamage(requestData.Caster, n.HERO, dmg, dmgType);
                        }
                    }
                    requestData.Caster.SpendAP(requireAP);
                    coolDownNow = coolDown;
                    return true;
                }
            }
            return false;
        }

        public override bool UpgradeSkill()
        {
            if (!upgraded)
            {
                upgraded = true;
                requireAP -= 1;
                dmg += 25;
                title = $"Издает устрашающий вопль, который пугает врагов и наносит им {dmg} маг. урона. Испуганные враги могут двигаться только в направлении от Вас.";
                return true;
            }
            return false;
        }
    }
}
