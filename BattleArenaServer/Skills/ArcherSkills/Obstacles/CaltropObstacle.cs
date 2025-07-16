using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Models;
using BattleArenaServer.Models.Obstacles;
using BattleArenaServer.Services;

namespace BattleArenaServer.Skills.Crossbowman.Obstacles
{
    public class CaltropObstacle : FillableObstacle
    {
        int instantDmg = 50;
        int bleedingDmg = 50;
        int bleedingDur = 2;
        public CaltropObstacle(int casterId, int hexId, int lifeTime, string team, int bleedingDamage, int bleedingDuration)
        {
            Name = "Caltrop";
            CasterId = casterId;
            HexId = hexId;
            LifeTime = lifeTime;
            Team = team;

            bleedingDmg = bleedingDamage;
            bleedingDur = bleedingDuration;
        }
        public override void ApplyEffect(Hero hero, Hex hex, GameData gameData)
        {
            if (hero.Team != Team)
            {
                //Убираем ловушку из игры
                hex.RemoveObstacle();
                gameData._obstacles.Remove(this);

                //Вешаем кровотечение
                BleedingDebuff bleedingDebuff = new BleedingDebuff(CasterId, bleedingDmg, bleedingDur);
                hero.AddEffect(bleedingDebuff, gameData);

                //Наносим мгновенный дамаг
                Hero? attacker = gameData._heroes.FirstOrDefault(x => x.Id == CasterId);
                AttackService.SetDamage(attacker, hero, instantDmg, Consts.DamageType.Pure, gameData);

            }
        }
    }
}
