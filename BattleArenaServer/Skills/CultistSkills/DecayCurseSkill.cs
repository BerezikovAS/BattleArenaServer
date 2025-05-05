using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.CultistSkills
{
    public class DecayCurseSkill : Skill
    {
        int radiusUpg = 0;
        public DecayCurseSkill()
        {
            name = "Decay Curse";
            dmg = 150;
            title = $"Насылает разложение на цель. Нансит {dmg} магического урона и запрещает восполнять здоровье.";
            titleUpg = "+1 к дальности. Разложение применяется и к противникам по соседству с целью.";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            range = 2;
            nonTarget = false;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.Target != null && requestData.TargetHex != null)
            {
                foreach (var hex in UtilityService.GetHexesRadius(requestData.TargetHex, radiusUpg))
                {
                    if (hex.HERO != null && hex.HERO.Team != requestData.Caster.Team)
                    {
                        DecayDebuff decayDebuff = new DecayDebuff(requestData.Caster.Id, 0, 2);
                        hex.HERO.AddEffect(decayDebuff);
                    }
                }

                AttackService.SetDamage(requestData.Caster, requestData.Target, dmg, dmgType);

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
                range += 1;
                stats.range += 1;
                radiusUpg += 1;
                title = $"Насылает разложение на цель. Нансит {dmg} магического урона и запрещает цели и противникам рядом с ней восполнять здоровье.";
                return true;
            }
            return false;
        }
    }
}
