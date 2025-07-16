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

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (requestData.Target != null && requestData.Caster != null && requestData.Caster.Team != requestData.Target.Team)
            {
                request = new EnemyTargetCastRequest();
                if (!request.startRequest(requestData, this, gameData))
                    return false;

                if (requestData.CasterHex == null || requestData.TargetHex == null)
                    return false;

                //Найдем гекс, позади нас, куда будем отпрыгивать
                Hex? moveHex = UtilityService.GetOneHexOnDirection(requestData.TargetHex, requestData.CasterHex, 2, gameData);
                if (moveHex != null && moveHex.IsFree())
                {
                    //Нашли, теперь отпрыгиваем
                    AttackService.MoveHero(requestData.Caster, requestData.CasterHex, moveHex, gameData);
                    //Ставим ловушку на освободившееся место, откуда отпрыгнули
                    CaltropObstacle caltropObstacle = new CaltropObstacle(requestData.Caster.Id, requestData.CasterHex.ID, 3, requestData.Caster.Team, bleedingDamage, bleedingDuration);
                    requestData.CasterHex.SetObstacle(caltropObstacle, gameData);

                    if (upgraded)
                    {
                        List<Hex> list = new List<Hex>();
                        list = gameData._hexes.FindAll(x => x.Distance(moveHex) == 1 && x.Distance(requestData.CasterHex) == 1);
                        foreach (var hex in list)
                            if (hex.IsFree() && hex.OBSTACLE == null)
                            {
                                CaltropObstacle caltropObstacle2 = new CaltropObstacle(requestData.Caster.Id, requestData.CasterHex.ID, 3, requestData.Caster.Team, bleedingDamage, bleedingDuration);
                                hex.SetObstacle(caltropObstacle2, gameData);
                            }
                    }

                    requestData.Caster.SpendAP(requireAP, gameData);
                    coolDownNow = coolDown;
                    return true;
                }
            }
            else
            {
                request = new HexTargetCastRequest();
                if (!request.startRequest(requestData, this, gameData))
                    return false;

                if (requestData.TargetHex != null && requestData.Caster != null && requestData.CasterHex != null)
                {
                    CaltropObstacle caltropObstacle = new CaltropObstacle(requestData.Caster.Id, requestData.TargetHex.ID, 3, requestData.Caster.Team, bleedingDamage, bleedingDuration);
                    requestData.TargetHex.SetObstacle(caltropObstacle, gameData);

                    if (upgraded)
                    {
                        List<Hex> list = new List<Hex>();
                        list = gameData._hexes.FindAll(x => x.Distance(requestData.CasterHex) == 1 && x.Distance(requestData.TargetHex) == 1);
                        foreach (var hex in list)
                            if (hex.IsFree() && hex.OBSTACLE == null)
                            {
                                CaltropObstacle caltropObstacle2 = new CaltropObstacle(requestData.Caster.Id, requestData.CasterHex.ID, 3, requestData.Caster.Team, bleedingDamage, bleedingDuration);
                                hex.SetObstacle(caltropObstacle2, gameData);
                            }
                    }

                    requestData.Caster.SpendAP(requireAP, gameData);
                    coolDownNow = coolDown;
                    return true;
                }
            }
            return false;
        }

        public override bool UpgradeSkill(GameData gameData)
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
