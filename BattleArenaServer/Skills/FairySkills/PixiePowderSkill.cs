using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.FairySkills
{
    public class PixiePowderSkill : Skill
    {
        int percentHeal = 5;
        int percentDmg = 30;
        public PixiePowderSkill()
        {
            name = "Pixie Powder";
            dmg = 0;
            title = $"Тратит все свои ОД и распыляет в области волшебную пыльцу. За каждое потраченное ОД союзники в области действия восстанавливают {percentHeal}% ХП" +
                $", а враги получают {percentDmg}% от их значения атаки в виде чистого урона.";
            titleUpg = "+1 к дальности, перезарядка равна потраченным ОД";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            nonTarget = false;
            range = 2;
            radius = 1;
            area = Consts.SpellArea.Radius;
            dmgType = Consts.DamageType.Pure;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new RangeAoECastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null)
            {
                int usedAP = requestData.Caster.AP;

                foreach (var n in UtilityService.GetHexesRadius(requestData.TargetHex, radius))
                {
                    if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                    {
                        int dealDmg = (int)(Convert.ToDouble(n.HERO.Dmg + n.HERO.StatsEffect.Dmg) * Convert.ToDouble(usedAP) * percentDmg / 100);
                        AttackService.SetDamage(requestData.Caster, n.HERO, dealDmg, dmgType);
                    }
                    else if (n.HERO != null && n.HERO.Team == requestData.Caster.Team)
                    {
                        int heal = (int)(Convert.ToDouble(n.HERO.MaxHP) * Convert.ToDouble(usedAP) * percentHeal / 100);
                        n.HERO.Heal(heal);
                    }
                }
                requestData.Caster.AP -= usedAP;

                if (upgraded)
                    coolDownNow = usedAP;
                else
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
                return true;
            }
            return false;
        }
    }
}
