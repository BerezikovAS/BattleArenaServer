using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.GolemSkills
{
    public class IronBlastSkill : Skill
    {
        int armorLoss = 1;
        int extraDmg = 10;
        public IronBlastSkill()
        {
            name = "Iron Blast";
            dmg = 100;
            title = $"Раскалывает свою броню, отчего та разлетается на осколки и поражает врагов вокруг.\n" +
                $"Теряет {armorLoss} брони и наносит врагам физический урон, равный {dmg} + {extraDmg} * броню";
            titleUpg = "+4 урона за броню, -1 ОД";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            radius = 1;
            area = Consts.SpellArea.Radius;
            dmgType = Consts.DamageType.Physical;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new NonTargerAoECastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (request.startRequest(requestData, this))
            {
                if (requestData.Caster != null && requestData.CasterHex != null)
                {
                    int totalDmg = dmg + requestData.Caster.Armor * extraDmg;
                    requestData.Caster.Armor -= armorLoss;

                    foreach (var n in UtilityService.GetHexesRadius(requestData.CasterHex, radius))
                    {
                        if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                        {
                            AttackService.SetDamage(requestData.Caster, n.HERO, totalDmg, dmgType);
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
                extraDmg += 4;
                title = $"Раскалывает свою броню, отчего та разлетается на осколки и поражает врагов вокруг.\n" +
                    $"Теряет {armorLoss} брони и наносит врагам физический урон, равный {dmg} + {extraDmg} * броню";
                return true;
            }
            return false;
        }
    }
}
