using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.WitchDoctorSkills
{
    public class HealingHerbsSkill : Skill
    {
        int regeneration = 60;
        public HealingHerbsSkill()
        {
            name = "Healing Herbs";
            title = $"Разбрасывает целебные травы в области. Союзники, попавшие под действие способности, получают регенерацию {regeneration} ХП на 2 хода.";
            titleUpg = "+20 к регенерации, -1 к восстановлению";
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
                        RegenerationBuff regenerationBuff = new RegenerationBuff(requestData.Caster.Id, regeneration, 2);
                        hex.HERO.AddEffect(regenerationBuff);
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
                regeneration += 20;
                title = $"Разбрасывает целебные травы в области. Союзники, попавшие под действие способности, получают регенерацию {regeneration} ХП на 2 хода.";
                return true;
            }
            return false;
        }
    }
}
