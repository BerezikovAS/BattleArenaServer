using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.DwarfSkills
{
    public class EngageSkill : Skill
    {
        int dmgBuff = 15;
        public EngageSkill()
        {
            name = "Engage";
            title = $"Вы и союзники в области получаете +{dmgBuff} к урону от атак.";
            titleUpg = "+1 к радиусу, +8 к урону";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            nonTarget = false;
            radius = 2;
            range = 0;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new NonTargerAoECastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (request.startRequest(requestData, this))
            {
                if (requestData.Caster != null && requestData.TargetHex != null)
                {
                    foreach (var n in UtilityService.GetHexesRadius(requestData.TargetHex, radius))
                    {
                        if (n.HERO != null && n.HERO.Team == requestData.Caster.Team)
                        {
                            EngageBuff engageBuff = new EngageBuff(requestData.Caster.Id, dmgBuff, 2);
                            n.HERO.AddEffect(engageBuff);
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
                radius += 1;
                stats.radius += 1;
                dmgBuff += 8;
                return true;
            }
            return false;
        }
    }
}
