namespace BattleArenaServer.Models.Obstacles
{
    public abstract class FillableObstacle : Obstacle
    {
        public abstract void ApplyEffect(Hero hero, Hex hex);
    }
}
