using BattleArenaServer.Models.Obstacles;
using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.VampireSkills.Obstacles
{
    public class CoffinObstacle : SolidObstacle
    {
        Hero hero;
        int heal;
        public CoffinObstacle(int id, int casterId, int hexId, int hp, string team, int lifeTime, int heal, Hero hero) : base(id, team, casterId, lifeTime)
        {
            Name = "Coffin";
            HexId = hexId;
            MaxHP = HP = hp;
            this.hero = hero;
            this.heal = heal;

            endLifeEffect += EndLifeCoffin;
            this.AddEffect -= this.BaseAddEffect;
            obstacleLifeTimeDecrease = Consts.ObstacleLifeTimeDecrease.StartTurn;
        }

        public void EndLifeCoffin(Hex currentHex)
        {
            if (HP > 0) // Истекло время действия, а не уничтожили
                hero.Heal(heal);

            currentHex.RemoveHero();
            currentHex.SetHero(hero);
        }
    }
}
