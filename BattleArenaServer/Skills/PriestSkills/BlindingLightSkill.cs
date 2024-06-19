using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.Priest
{
    public class BlindingLightSkill : Skill
    {
        int dmg = 100;
        public BlindingLightSkill()
        {
            name = "BlindingLight";
            title = $"Враги в области действия теряют {dmg} ХП и получают ослепление";
            titleUpg = "+40 к урону, +1 к дальности";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 2;
            radius = 1;
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
                foreach (var n in UtilityService.GetHexesRadius(requestData.TargetHex, radius))
                {
                    if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                    {
                        BlindDebuff blindDebuff = new BlindDebuff(requestData.Caster.Id, 0, 2);
                        n.HERO.AddEffect(blindDebuff);
                        blindDebuff.ApplyEffect(n.HERO);
                        AttackService.SetDamage(requestData.Caster, n.HERO, dmg, Consts.DamageType.Pure);
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
                dmg += 40;
                stats.range += 1;
                title = $"Враги в области действия теряют {dmg} ХП и получают ослепление";
                return true;
            }
            return false;
        }
    }
}
