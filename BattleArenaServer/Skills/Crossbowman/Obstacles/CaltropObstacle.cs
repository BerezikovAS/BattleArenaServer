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

            //Одновременно может существовать только одна ловушка. Если она уже на поле, уберем её
            Obstacle? obst = GameData._obstacles.FirstOrDefault(x => x.Name == "Caltrop");
            if (obst != null)
            {
                Hex? hex = GameData._hexes.FirstOrDefault(x => x.ID == obst.HexId);
                if (hex != null && hex.OBSTACLE != null)
                {
                    hex.RemoveObstacle();
                    GameData._obstacles.Remove(obst);
                }
            }
        }
        public override void ApplyEffect(Hero hero, Hex hex)
        {
            if (hero.Team != Team)
            {
                //Убираем ловушку из игры
                hex.RemoveObstacle();
                GameData._obstacles.Remove(this);

                //Вешаем кровотечение
                BleedingDebuff bleedingDebuff = new BleedingDebuff(CasterId, bleedingDmg, bleedingDur);
                hero.EffectList.Add(bleedingDebuff);

                //Наносим мгновенный дамаг
                Hero? attacker = GameData._heroes.FirstOrDefault(x => x.Id == CasterId);
                AttackService.SetDamage(attacker, hero, instantDmg, Consts.DamageType.Pure);

            }
        }
    }
}
