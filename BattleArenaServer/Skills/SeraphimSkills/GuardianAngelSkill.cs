using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.SeraphimSkills
{
    public class GuardianAngelSkill : Skill
    {
        public GuardianAngelSkill()
        {
            name = "Guardian Angel";
            title = $"Ангелы-хранители полностью защищают от физического урона Вас и союзников в области.";
            titleUpg = "+1 к дальности, +1 к радиусу";
            coolDown = 5;
            coolDownNow = 0;
            requireAP = 1;
            radius = 1;
            range = 2;
            nonTarget = false;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new RangeAoECastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null)
            {
                foreach (var hex in UtilityService.GetHexesRadius(requestData.TargetHex, radius))
                {
                    if (hex.HERO != null && hex.HERO.Team == requestData.Caster.Team)
                    {
                        GuardianAngelBuff guardianAngelBuff = new GuardianAngelBuff(requestData.Caster.Id, 0, 2);
                        hex.HERO.AddEffect(guardianAngelBuff);
                    }
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
                range += 1;
                stats.range += 1;
                radius += 1;
                stats.radius += 1;
                return true;
            }
            return false;
        }
    }
}
