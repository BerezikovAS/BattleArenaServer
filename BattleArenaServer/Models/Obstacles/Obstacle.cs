namespace BattleArenaServer.Models.Obstacles
{
    public abstract class Obstacle
    {
        public string Name { get; set; } = "obstacle";
        public string Team { get; set; } = "red";
        public int HexId { get; set; }
        public int CasterId { get; set; }
        public int LifeTime { get; set; }
    }
}
