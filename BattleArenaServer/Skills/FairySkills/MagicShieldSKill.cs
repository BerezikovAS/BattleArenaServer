using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.FairySkills
{
    public class MagicShieldSKill : Skill
    {
        int shieldDurability = 100;
        public MagicShieldSKill()
        {
            name = "Magic Shield";
            title = $"Вы и союзники в области действия получают магический щит, который имеет {shieldDurability} прочности.";
            titleUpg = "+1 к дальности, +20 к прочности щитов";
            coolDown = 4;
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
                        MagicShieldBuff magicShieldBuff = new MagicShieldBuff(requestData.Caster.Id, shieldDurability, 2);
                        hex.HERO.AddEffect(magicShieldBuff);
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
                shieldDurability += 20;
                title = $"Вы и союзники в области действия получают магический щит, который имеет {shieldDurability} прочности.";
                return true;
            }
            return false;
        }
    }
}
