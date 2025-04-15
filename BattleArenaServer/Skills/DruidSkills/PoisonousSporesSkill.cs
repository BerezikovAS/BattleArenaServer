using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.DruidSkills
{
    public class PoisonousSporesSkill : Skill
    {
        int percentHPLoss = 8;
        int poisonDuration = 2;
        public PoisonousSporesSkill()
        {
            name = "Poisonous Spores";
            dmg = 100;
            title = $"Выбрасывает вперед ядовитые споры, которые наносят {dmg} маг. урона и заражают врагов, отчего те теряют {percentHPLoss}% ХП в ход." +
                $" Действует {poisonDuration} хода.";
            titleUpg = "+30 к урону, +3% к потере ХП в ход.";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            range = 3;
            radius = 3;
            nonTarget = false;
            area = Consts.SpellArea.WideLine;
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
                foreach (var n in UtilityService.GetHexesWideLine(requestData.CasterHex, requestData.TargetHex, radius))
                {
                    if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                    {
                        PoisonDebuff poisonDebuff = new PoisonDebuff(requestData.Caster.Id, percentHPLoss, poisonDuration + 1);
                        n.HERO.AddEffect(poisonDebuff);

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
                dmg += 30;
                percentHPLoss += 3;
                title = $"Выбрасывает вперед ядовитые споры, которые наносят {dmg} маг. урона и заражают врагов, отчего те теряют {percentHPLoss}% ХП в ход." +
                $" Действует {poisonDuration} хода.";
                return true;
            }
            return false;
        }
    }
}
