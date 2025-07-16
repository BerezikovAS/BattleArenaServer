using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.PlagueDoctorSkills
{
    public class TuberculosisSkill : Skill
    {
        int dmgPerSkill = 30;
        public TuberculosisSkill()
        {
            name = "Tuberculosis";
            dmg = 150;
            title = $"Враг поражается болезнью и получает {dmg} маг. урона. Каждое применение способности отнимает у врага {dmgPerSkill} ХП и 1 ОД.";
            titleUpg = "+10 к потере ХП за способность, -1 к перезарядке";
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

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (!request.startRequest(requestData, this, gameData))
                return false;

            if (requestData.Caster != null && requestData.Target != null)
            {
                TuberculosisDebuff tuberculosisDebuff = new TuberculosisDebuff(requestData.Caster.Id, dmgPerSkill, 2, requestData.Caster);
                requestData.Target.AddEffect(tuberculosisDebuff, gameData);

                AttackService.SetDamage(requestData.Caster, requestData.Target, dmg, dmgType, gameData);

                requestData.Caster.SpendAP(requireAP, gameData);
                coolDownNow = coolDown;
                return true;
            }
            return false;
        }

        public override bool UpgradeSkill(GameData gameData)
        {
            if (!upgraded)
            {
                upgraded = true;
                dmgPerSkill += 10;
                coolDown -= 1;
                stats.coolDown -= 1;
                title = $"Враг поражается болезнью и получает {dmg} маг. урона. Каждое применение способности отнимает у врага {dmgPerSkill} ХП и 1 ОД.";
                return true;
            }
            return false;
        }
    }
}
