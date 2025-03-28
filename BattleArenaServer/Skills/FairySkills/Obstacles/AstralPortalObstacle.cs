using BattleArenaServer.Models.Obstacles;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.Effects.Debuffs;

namespace BattleArenaServer.Skills.FairySkills.Obstacles
{
    public class AstralPortalObstacle : FillableObstacle
    {
        int Dmg;
        public AstralPortalObstacle(int casterId, int hexId, int lifeTime, string team, int dmg)
        {
            Name = "AstralPortal";
            CasterId = casterId;
            HexId = hexId;
            LifeTime = lifeTime;
            Team = team;
            Dmg = dmg;
        }
        public override void ApplyEffect(Hero hero, Hex hex)
        {
            if (hero.Team != Team)
            {
                //Наносим мгновенный дамаг
                Hero? attacker = GameData._heroes.FirstOrDefault(x => x.Id == CasterId);
                AttackService.SetDamage(attacker, hero, Dmg, Consts.DamageType.Pure);

                //Вешаем безмолвие
                SilenceDebuff silenceDebuff = new SilenceDebuff(CasterId, 0, 2);
                hero.AddEffect(silenceDebuff);
            }
            //Убираем портал из игры
            hex.RemoveObstacle();
            GameData._obstacles.Remove(this);
        }
    }
}
