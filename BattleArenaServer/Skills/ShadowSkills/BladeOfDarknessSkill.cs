using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.ShadowSkills
{
    public class BladeOfDarknessSkill : Skill
    {
        int percentDmg = 12;
        public BladeOfDarknessSkill()
        {
            name = "Blade Of Darkness";
            dmg = 60;
            title = $"Атака из тьмы наносит врагу магический урон, равный {dmg} + {percentDmg}% от текущего ХП цели. Также накладывает слепоту.";
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

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (!request.startRequest(requestData, this, gameData))
                return false;

            if (requestData.Caster != null && requestData.Target != null)
            {
                int dmgDeal = dmg + (int)(Convert.ToDouble(requestData.Target.HP * percentDmg) / 100);

                BlindDebuff blindDebuff = new BlindDebuff(requestData.Caster.Id, 0, 2);
                requestData.Target.AddEffect(blindDebuff, gameData);
                
                AttackService.SetDamage(requestData.Caster, requestData.Target, dmgDeal, dmgType, gameData);
                
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
                percentDmg += 10;
                title = $"Атака из тьмы наносит врагу магический урон, равный {dmg} + {percentDmg}% от текущего ХП цели. Также накладывает слепоту.";
                return true;
            }
            return false;
        }
    }
}
