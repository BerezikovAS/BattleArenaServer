using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.FallenKingSkills
{
    public class DepletionAbsorptionSkill : Skill
    {
        int shieldPerSkill = 20;
        public DepletionAbsorptionSkill()
        {
            name = "Depletion Absorption";
            dmg = 125;
            title = $"Враги в конусе получают {dmg} маг. урона, а Вы за каждую способность Врага в откате получаете щит на {shieldPerSkill} прочности.";
            titleUpg = "+25 урона, +5 к прочности щита";
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
                int countCDSkills = 0;
                foreach (var n in UtilityService.GetHexesSmallCone(requestData.CasterHex, requestData.TargetHex, radius))
                {
                    if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                    {
                        if (n.HERO.IsMainHero)
                            foreach (var skill in n.HERO.SkillList)
                                if (skill.coolDownNow > 0)
                                    countCDSkills++;

                        AttackService.SetDamage(requestData.Caster, n.HERO, dmg, dmgType);
                    }
                }

                if (countCDSkills > 0)
                {
                    DmgShieldBuff dmgShieldBuff = new DmgShieldBuff(requestData.Caster.Id, countCDSkills * shieldPerSkill, 2);
                    requestData.Caster.AddEffect(dmgShieldBuff);
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
                dmg += 25;
                shieldPerSkill += 5;
                title = $"Враги в конусе получают {dmg} маг. урона, а Вы за каждую способность Врага в откате получаете щит на {shieldPerSkill} прочности.";
                return true;
            }
            return false;
        }
    }
}
