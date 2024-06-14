using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;
using BattleArenaServer.Services;
using BattleArenaServer.Skills.Crossbowman.Obstacles;

namespace BattleArenaServer.Skills.Crossbowman
{
    public class CaltropSkill : Skill
    {
        int bleedingDamage = 50;
        int bleedingDuration = 2;
        public CaltropSkill()
        {
            name = "Caltrop";
            title = $"Устанавливает ловушку с колючками перед собой.\nЕсли применить на врага, то герой сначала отпрыгнет от него." +
                $"\nЛовушка наносит 50 чистого урона сразу и вызывает кровотечение на 2 хода, наносящее по {bleedingDamage} урона каждый ход. Ловушка исчезнет через 3 хода.";
            titleUpg = "+20 к урону от кровотечения. Кровотечение длится 3 хода.";
            coolDown = 3;
            coolDownNow = 0;
            requireAP = 1;
            range = 1;
            nonTarget = false;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public override bool Cast(RequestData requestData)
        {
            if (requestData.Target != null && requestData.Caster != null && requestData.Caster.Team != requestData.Target.Team)
            {
                request = new EnemyTargetCastRequest();
                if (!request.startRequest(requestData, this))
                    return false;

                if (requestData.CasterHex == null || requestData.TargetHex == null)
                    return false;

                //Найдем гекс, позади нас, куда будем отпрыгивать
                Hex? moveHex = UtilityService.GetOneHexOnDirection(requestData.TargetHex, requestData.CasterHex, 2);
                if (moveHex != null && moveHex.IsFree())
                {
                    //Нашли, теперь отпрыгиваем
                    AttackService.MoveHero(requestData.Caster, requestData.CasterHex, moveHex);
                    //Ставим ловушку на освободившееся место, откуда отпрыгнули
                    CaltropObstacle caltropObstacle = new CaltropObstacle(requestData.Caster.Id, requestData.CasterHex.ID, 3, requestData.Caster.Team, bleedingDamage, bleedingDuration);
                    requestData.CasterHex.SetObstacle(caltropObstacle);

                    requestData.Caster.AP -= requireAP;
                    coolDownNow = coolDown;
                    return true;
                }
            }
            else
            {
                request = new HexTargetCastRequest();
                if (!request.startRequest(requestData, this))
                    return false;

                if (requestData.TargetHex != null && requestData.Caster != null)
                {
                    CaltropObstacle caltropObstacle = new CaltropObstacle(requestData.Caster.Id, requestData.TargetHex.ID, 3, requestData.Caster.Team, bleedingDamage, bleedingDuration);
                    requestData.TargetHex.SetObstacle(caltropObstacle);

                    requestData.Caster.AP -= requireAP;
                    coolDownNow = coolDown;
                    return true;
                }
            }
            return false;
        }

        public override bool UpgradeSkill()
        {
            if (!upgraded)
            {
                upgraded = true;
                bleedingDamage += 20;
                bleedingDuration += 1;
                return true;
            }
            return false;
        }
    }
}
