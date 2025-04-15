using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.NecromancerSkills
{
    public class LifeDrainSkill : Skill
    {
        private int percentHeal = 50;

        public LifeDrainSkill()
        {
            name = "Life Drain";
            dmg = 115;
            title = $"Высасывает жизненные силы из врагов, нанося им {dmg} магического урона и восполняя себе ХП, равное {percentHeal}% от нанесенного ущерба.";
            titleUpg = "+25 урона, +15% к восстановлению ХП";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 3;
            radius = 3;
            area = Consts.SpellArea.SmallConus;
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
                requestData.Caster.DamageDealed = 0;
                foreach (var n in UtilityService.GetHexesSmallCone(requestData.CasterHex, requestData.TargetHex, radius))
                {
                    if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                    {
                        AttackService.SetDamage(requestData.Caster, n.HERO, dmg, dmgType);
                    }
                }
                int heal = Convert.ToInt32(Convert.ToDouble(requestData.Caster.DamageDealed * percentHeal) / 100);
                requestData.Caster.Heal(heal);

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
                dmg += 25;
                percentHeal += 15;
                return true;
            }
            return false;
        }
    }
}
