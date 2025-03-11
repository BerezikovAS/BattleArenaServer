using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Models.Summons;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.NecromancerSkills
{
    public class LivingDeadSkill : Skill
    {
        int skeletonHP = 250;
        int lifeTime = 4;
        int armor = 1;
        int resist = 0;
        int attackRadius = 1;

        public LivingDeadSkill()
        {
            name = "Living Dead";
            dmg = 45;
            title = $"Поднимает к жизни скелета-воина из-под земли, который сражается на Вашей стороне.";
            titleUpg = "Поднимает скелета-лучника.";
            coolDown = 6;
            coolDownNow = 0;
            requireAP = 2;
            range = 2;
            nonTarget = false;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Physical;
        }

        public new ISkillCastRequest request => new HexTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null && requestData.TargetHex.OBSTACLE == null)
            {
                //Вызываем скелета
                int Id = GameData._hexes.Max(x => x.HERO != null ? x.HERO.Id : 0) + 1;
                SkeletonSummon skeleton = new SkeletonSummon(Id, requestData.Caster.Team, skeletonHP, armor, resist, attackRadius, dmg);
                requestData.TargetHex.SetHero(skeleton);
                GameData._heroes.Add(skeleton);

                //Обновим ауры
                AttackService.ContinuousAuraAction();

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
                dmg += 5;
                attackRadius += 2;
                title = $"Поднимает к жизни скелета-лучника из-под земли, который сражается на Вашей стороне.";
                return true;
            }
            return false;
        }
    }
}
