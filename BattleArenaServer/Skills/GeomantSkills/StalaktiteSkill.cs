using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;
using BattleArenaServer.Skills.GeomantSkills.Obstacles;

namespace BattleArenaServer.Skills.GeomantSkills
{
    public class StalaktiteSkill : Skill
    {
        int dmg = 100;
        int lifeTime = 3;
        public StalaktiteSkill()
        {
            name = "Stalaktite";
            title = $"Из-под земли вырывается каменный шпиль, нанося {dmg} магического урона врагам вокруг. Шпиль блокирует перемещение и существует {lifeTime} хода.";
            titleUpg = "+40 к урону, +1 к времени жизни столоктита.";
            coolDown = 1;
            coolDownNow = 0;
            requireAP = 2;
            range = 3;
            nonTarget = false;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new HexTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null)
            {
                //Ставим столоктит
                StalaktiteObstacle stalaktiteObstacle = new StalaktiteObstacle(requestData.Caster.Id, requestData.TargetHex.ID, lifeTime, requestData.Caster.Team);
                requestData.TargetHex.SetObstacle(stalaktiteObstacle);

                //Наносим урон врагам вокруг него
                foreach (var hex in UtilityService.GetHexesRadius(requestData.TargetHex, 1))
                {
                    if (hex.HERO != null && hex.HERO.Team != requestData.Caster.Team)
                        AttackService.SetDamage(requestData.Caster, hex.HERO, dmg, Consts.DamageType.Magic);
                }
                
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
                dmg += 40;
                lifeTime += 1;
                return true;
            }
            return false;
        }
    }
}
