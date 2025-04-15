using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.CultistSkills
{
    public class WeakeningWaveSkill : Skill
    {
        int percentDmgReduction = 40;
        public WeakeningWaveSkill()
        {
            name = "Weakening Wave";
            dmg = 75;
            title = $"Ослабляющая волна уменьшает врагам урон от атак на {percentDmgReduction}% и наносит им {dmg} магического урона.";
            titleUpg = "+20% к ослаблению, +1 к дальности.";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            range = 2;
            radius = 2;
            nonTarget = false;
            area = Consts.SpellArea.Conus;
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
                foreach (var n in UtilityService.GetHexesCone(requestData.CasterHex, requestData.TargetHex, radius))
                {
                    if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                    {
                        WeaknessDebuff weaknessDebuff = new WeaknessDebuff(requestData.Caster.Id, percentDmgReduction, 2);
                        n.HERO.AddEffect(weaknessDebuff);

                        AttackService.SetDamage(requestData.Caster, n.HERO, dmg, dmgType);
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
                percentDmgReduction += 20;
                title = $"Ослабляющая волна уменьшает врагам урон от атак на {percentDmgReduction}% и наносит им {dmg} магического урона.";
                return true;
            }
            return false;
        }
    }
}
