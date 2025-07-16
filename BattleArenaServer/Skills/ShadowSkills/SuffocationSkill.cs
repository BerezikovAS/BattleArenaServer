using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.ShadowSkills
{
    public class SuffocationSkill : Skill
    {
        public SuffocationSkill()
        {
            name = "Suffocation";
            dmg = 140;
            title = $"Сгустки тени удушают цель, отчего та не может применять заклинания и получает {dmg} магического урона.";
            titleUpg = "+1 к дальности, +30 к урону";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 2;
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
                SilenceDebuff silenceDebuff = new SilenceDebuff(requestData.Caster.Id, 0, 2);
                requestData.Target.AddEffect(silenceDebuff, gameData);
                
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
                dmg += 30;
                range += 1;
                stats.range += 1;
                title = $"Сгустки тени удушают цель, отчего та не может применять заклинания и получает {dmg} магического урона.";
                return true;
            }
            return false;
        }
    }
}
