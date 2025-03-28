using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.ShadowSkills
{
    public class BladeOfDarknessSkill : Skill
    {
        int percentDmg = 15;
        public BladeOfDarknessSkill()
        {
            name = "Blade Of Darkness";
            title = $"Атака из тьмы наносит врагу магический урон, равный {percentDmg}% от текущего ХП цели. Также накладывает слепоту.";
            titleUpg = "+10% к урону от текущего ХП цели.";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 1;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.Target != null)
            {
                int dmg = (int)(Convert.ToDouble(requestData.Target.HP * percentDmg) / 100);
                AttackService.SetDamage(requestData.Caster, requestData.Target, dmg, dmgType);

                BlindDebuff blindDebuff = new BlindDebuff(requestData.Caster.Id, 0, 2);
                requestData.Target.AddEffect(blindDebuff);
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
                percentDmg += 10;
                title = $"Атака из тьмы наносит врагу магический урон, равный {percentDmg}% от текущего ХП цели. Также накладывает слепоту.";
                return true;
            }
            return false;
        }
    }
}
