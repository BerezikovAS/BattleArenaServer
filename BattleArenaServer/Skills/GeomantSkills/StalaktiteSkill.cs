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
            dmg = 65;
            title = $"Из-под земли вырывается каменный шпиль, нанося {dmg} магического урона врагам вокруг. Шпиль блокирует перемещение, имеет {stalaktiteHP} ХП и существует {lifeTime - 1} хода.";
            titleUpg = "+20 к урону, +40 к ХП.";
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

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (!request.startRequest(requestData, this, gameData))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null && requestData.TargetHex.OBSTACLE == null)
            {
                //Ставим столоктит
                int Id = gameData._heroes.Max(x => x.Id) + 1;
                StalaktiteObstacle stalaktiteObstacle = new StalaktiteObstacle(Id, requestData.Caster.Id, requestData.TargetHex.ID, stalaktiteHP, requestData.Caster.Team, lifeTime);
                requestData.TargetHex.SetHero(stalaktiteObstacle);
                gameData._solidObstacles.Add(stalaktiteObstacle);

                //Наносим урон врагам вокруг него
                foreach (var hex in UtilityService.GetHexesRadius(requestData.TargetHex, 1, gameData))
                {
                    if (hex.HERO != null && hex.HERO.Team != requestData.Caster.Team)
                        AttackService.SetDamage(requestData.Caster, hex.HERO, dmg, dmgType, gameData);
                }

                //Обновим ауры
                AttackService.ContinuousAuraAction(gameData);

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
                stalaktiteHP += 40;
                title = $"Из-под земли вырывается каменный шпиль, нанося {dmg} магического урона врагам вокруг. Шпиль блокирует перемещение, имеет {stalaktiteHP} ХП и существует {lifeTime - 1} хода.";
                return true;
            }
            return false;
        }
    }
}
