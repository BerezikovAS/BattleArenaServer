using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;
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
                $"\nЛовушка наносит 50 чистого урона сразу и вызывает кровотечение на {bleedingDuration} хода, наносящее по {bleedingDamage} урона каждый ход. Ловушка исчезнет через 3 хода.";
            titleUpg = "Устанавливает еще 2 дополнительные ловушки рядом";
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

                    if (upgraded)
                    {
                        List<Hex> list = new List<Hex>();
                        list = GameData._hexes.FindAll(x => x.Distance(moveHex) == 1 && x.Distance(requestData.CasterHex) == 1);
                        foreach (var hex in list)
                            if (hex.IsFree() && hex.OBSTACLE == null)
                            {
                                CaltropObstacle caltropObstacle2 = new CaltropObstacle(requestData.Caster.Id, requestData.CasterHex.ID, 3, requestData.Caster.Team, bleedingDamage, bleedingDuration);
                                hex.SetObstacle(caltropObstacle2);
                            }
                    }

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

                if (requestData.TargetHex != null && requestData.Caster != null && requestData.CasterHex != null)
                {
                    CaltropObstacle caltropObstacle = new CaltropObstacle(requestData.Caster.Id, requestData.TargetHex.ID, 3, requestData.Caster.Team, bleedingDamage, bleedingDuration);
                    requestData.TargetHex.SetObstacle(caltropObstacle);

                    if (upgraded)
                    {
                        List<Hex> list = new List<Hex>();
                        list = GameData._hexes.FindAll(x => x.Distance(requestData.CasterHex) == 1 && x.Distance(requestData.TargetHex) == 1);
                        foreach (var hex in list)
                            if (hex.IsFree() && hex.OBSTACLE == null)
                            {
                                CaltropObstacle caltropObstacle2 = new CaltropObstacle(requestData.Caster.Id, requestData.CasterHex.ID, 3, requestData.Caster.Team, bleedingDamage, bleedingDuration);
                                hex.SetObstacle(caltropObstacle2);
                            }
                    }

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
                title = $"Устанавливает 3 ловушки с колючками перед собой.\nЕсли применить на врага, то герой сначала отпрыгнет от него." +
                $"\nЛовушка наносит 50 чистого урона сразу и вызывает кровотечение на {bleedingDuration} хода, наносящее по {bleedingDamage} урона каждый ход. Ловушка исчезнет через 3 хода.";
                return true;
            }
            return false;
        }
    }
}
