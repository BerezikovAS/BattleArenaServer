using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.SeraphimSkills
{
    public class GreateJudgementSkill : Skill
    {
        public GreateJudgementSkill()
        {
            name = "Greate Judgement";
            dmg = 60;
            title = $"Цель предстает перед великим судом, отчего получает {dmg} чистого урона и весь входящий урон также становится чистым.";
            titleUpg = "+20 к урону, -1 к перезарядке";
            coolDown = 5;
            coolDownNow = 0;
            requireAP = 1;
            nonTarget = false;
            range = 2;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Pure;
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (!request.startRequest(requestData, this, gameData))
                return false;

            if (requestData.Caster != null && requestData.Target != null)
            {
                GreateJudgementDebuff greateJudgementDebuff = new GreateJudgementDebuff(requestData.Caster.Id, 0, 1);
                requestData.Target.AddEffect(greateJudgementDebuff, gameData);
                
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
                dmg += 20;
                coolDown -= 1;
                stats.coolDown -= 1;
                title = $"Цель предстает перед великим судом, отчего получает {dmg} чистого урона и весь входящий урон также становится чистым.";
                return true;
            }
            return false;
        }
    }
}
