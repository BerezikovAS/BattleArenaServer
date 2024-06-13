using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;
using System.Xml.Linq;
using System;
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

        public override bool Cast(Hero caster, Hero? target, Hex? targetHex)
        {
            if (target != null && caster.Team != target.Team)
            {
                Hex? casterHex = GameData._hexes.FirstOrDefault(x => x.ID == caster.HexId);
                request = new EnemyTargetCastRequest();
                if (request.startRequest(caster, target, targetHex, this) && casterHex != null && targetHex != null)
                {
                    //Найдем гекс, позади нас, куда будем отпрыгивать
                    Hex? moveHex = UtilityService.GetOneHexOnDirection(targetHex, casterHex, 2);
                    if (moveHex != null && moveHex.HERO == null)
                    {
                        //Нашли, теперь отпрыгиваем
                        AttackService.MoveHero(caster, casterHex, moveHex);
                        //Ставим ловушку на освободившееся место, откуда отпрыгнули
                        CaltropObstacle caltropObstacle = new CaltropObstacle(caster.Id, casterHex.ID, 3, caster.Team, bleedingDamage, bleedingDuration);
                        casterHex.SetObstacle(caltropObstacle);

                        caster.AP -= requireAP;
                        coolDownNow = coolDown;
                        return true;
                    }
                }
            }
            else
            {
                request = new HexTargetCastRequest();
                if (request.startRequest(caster, target, targetHex, this))
                {
                    if (targetHex != null)
                    {
                        CaltropObstacle caltropObstacle = new CaltropObstacle(caster.Id, targetHex.ID, 3, caster.Team, bleedingDamage, bleedingDuration);
                        targetHex.SetObstacle(caltropObstacle);

                        caster.AP -= requireAP;
                        coolDownNow = coolDown;
                        return true;
                    }
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
