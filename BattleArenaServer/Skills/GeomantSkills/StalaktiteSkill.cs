using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;
using BattleArenaServer.Skills.GeomantSkills.Obstacles;

namespace BattleArenaServer.Skills.GeomantSkills
{
    public class StalaktiteSkill : Skill
    {
        int stalaktiteHP = 80;
        int lifeTime = 4;

        public StalaktiteSkill()
        {
            name = "Stalaktite";
            dmg = 80;
            title = $"Из-под земли вырывается каменный шпиль, нанося {dmg} магического урона врагам вокруг. Шпиль блокирует перемещение, имеет {stalaktiteHP} ХП и существует {lifeTime - 1} хода.";
            titleUpg = "+40 к урону, +40 к ХП.";
            coolDown = 1;
            coolDownNow = 0;
            requireAP = 2;
            range = 2;
            nonTarget = false;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new HexTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null && requestData.TargetHex.OBSTACLE == null)
            {
                //Ставим столоктит
                int Id = GameData._hexes.Max(x => x.HERO != null ? x.HERO.Id : 0) + 1;
                StalaktiteObstacle stalaktiteObstacle = new StalaktiteObstacle(Id, requestData.Caster.Id, requestData.TargetHex.ID, stalaktiteHP, requestData.Caster.Team, lifeTime);
                requestData.TargetHex.SetHero(stalaktiteObstacle);
                GameData._solidObstacles.Add(stalaktiteObstacle);
                //GameData._heroes.Add(stalaktiteObstacle);

                //Наносим урон врагам вокруг него
                foreach (var hex in UtilityService.GetHexesRadius(requestData.TargetHex, 1))
                {
                    if (hex.HERO != null && hex.HERO.Team != requestData.Caster.Team)
                        AttackService.SetDamage(requestData.Caster, hex.HERO, dmg, dmgType);
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
                stalaktiteHP += 40;
                title = $"Из-под земли вырывается каменный шпиль, нанося {dmg} магического урона врагам вокруг. Шпиль блокирует перемещение, имеет {stalaktiteHP} ХП и существует {lifeTime - 1} хода.";
                return true;
            }
            return false;
        }
    }
}
