using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.InvokerSkills
{
    public class ManaBlastSkill : Skill
    {
        private int percentDmg = 17;
        public ManaBlastSkill()
        {
            name = "Mana Blast";
            dmg = 0;
            title = $"Резкий выплеск маны провоцирует магический взрыв. Враги в области получают магический урон равный {percentDmg}% от их максимального ХП.";
            titleUpg = "+6% к урону от макс. ХП";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 2;
            radius = 1;
            area = Consts.SpellArea.Radius;
            dmgType = Consts.DamageType.Magic;
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
                        int resDmg = (int)(Convert.ToDouble(n.HERO.MaxHP) * Convert.ToDouble(percentDmg) / 100);
                        AttackService.SetDamage(requestData.Caster, n.HERO, resDmg, Consts.DamageType.Magic);
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
                percentDmg += 6;
                title = $"Резкий выплеск маны провоцирует магический взрыв. Враги в области получают магический урон равный {percentDmg}% от их максимального ХП.";
                return true;
            }
            return false;
        }
    }
}
