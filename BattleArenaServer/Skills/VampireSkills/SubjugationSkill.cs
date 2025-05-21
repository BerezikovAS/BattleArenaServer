using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Models.Summons;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;
using static BattleArenaServer.Models.Consts;

namespace BattleArenaServer.Skills.VampireSkills
{
    public class SubjugationSkill : Skill
    {
        int loseAP = 1;
        int extraHP = 100;
        public SubjugationSkill()
        {
            name = "Subjugation";
            dmg = 100;
            title = $"Берет вражеское призванное существо под свой контроль и продляет его жизнь на 1 ход." +
                $" Если это был герой, то наносит тому {dmg} маг. урона и отнимает {loseAP} ОД.";
            titleUpg = $"-1 к перезарядке. Призванное существо получает +{extraHP} к запасу ХП";
            coolDown = 5;
            coolDownNow = 0;
            range = 2;
            requireAP = 1;
            nonTarget = false;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = DamageType.Magic;
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Target != null && requestData.Caster != null)
            {
                if (requestData.Target is Summon)
                {
                    (requestData.Target as Summon).casterId = requestData.Caster.Id;
                    (requestData.Target as Summon).lifeTime++;
                    requestData.Target.Team = requestData.Caster.Team;

                    if (upgraded)
                    {
                        requestData.Target.MaxHP += extraHP;
                        requestData.Target.HP += extraHP;
                    }
                }
                else
                {
                    requestData.Target.AP -= loseAP;
                    if (requestData.Target.AP < 0)
                        requestData.Target.AP = 0;

                    AttackService.SetDamage(requestData.Caster, requestData.Target, dmg, dmgType);
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
                coolDown -= 1;
                stats.coolDown -= 1;
                title = $"Берет вражеское призванное существо под свой контроль и продляет его жизнь на 1 ход и даёт +{extraHP} к макс ХП." +
                    $" Если это был герой, то наносит тому {dmg} маг. урона и отнимает {loseAP} ОД.";
                return true;
            }
            return false;
        }
    }
}
