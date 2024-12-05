using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.AssassinSkills
{
    public class SmokeBombSkill : Skill
    {
        int extraArmor = 3;
        public SmokeBombSkill()
        {
            name = "Smoke Bomb";
            dmg = 100;
            title = $"Бросает дымовую гранату, ослепляя врагов, и скрывается из вида, получая +{extraArmor} брони. Скрытность не даёт выбрать Вас целью атаки или заклинания.";
            titleUpg = $"Также наносит {dmg} чистого урона ослепленным врагам.";
            coolDown = 5;
            coolDownNow = 0;
            requireAP = 1;
            nonTarget = false;
            radius = 1;
            range = 0;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Pure;
        }

        public new ISkillCastRequest request => new NonTargerAoECastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null)
            {
                foreach (var hex in UtilityService.GetHexesRadius(requestData.TargetHex, 1))
                {
                    if (hex.HERO != null && hex.HERO.Team != requestData.Caster.Team)
                    {
                        BlindDebuff blindDebuff = new BlindDebuff(requestData.Caster.Id, 0, 2);
                        hex.HERO.AddEffect(blindDebuff);

                        if (upgraded)
                            AttackService.SetDamage(requestData.Caster, hex.HERO, dmg, dmgType);
                    }
                }

                SmokeBuff smokeBuff = new SmokeBuff(requestData.Caster.Id, extraArmor, 2);
                requestData.Caster.AddEffect(smokeBuff);

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
                title = $"Бросает дымовую гранату, ослепляя врагов, и скрывается из вида, получая +{extraArmor} брони. " +
                    $"Скрытность не даёт выбрать Вас целью атаки или заклинания. Также наносит {dmg} ослепленным врагам.";
                return true;
            }
            return false;
        }
    }
}
