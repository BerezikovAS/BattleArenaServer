using BattleArenaServer.Models.Obstacles;

namespace BattleArenaServer.Skills.GeomantSkills.Obstacles
{
    public class StalaktiteObstacle : SolidObstacle
    {
        public StalaktiteObstacle(int casterId, int hexId, int lifeTime, string team)
        {
            Name = "Stalaktite";
            CasterId = casterId;
            HexId = hexId;
            LifeTime = lifeTime;
            Team = team;
        }
    }
}
