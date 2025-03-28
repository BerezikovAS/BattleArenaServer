using BattleArenaServer.Effects;
using BattleArenaServer.Models.Obstacles;

namespace BattleArenaServer.Skills.GeomantSkills.Obstacles
{
    public class StalaktiteObstacle : SolidObstacle
    {
        public StalaktiteObstacle(int id, int casterId, int hexId, int hp, string team, int lifeTime) : base(id, team, casterId, lifeTime)
        {
            Name = "Stalaktite";
            HexId = hexId;
            MaxHP = HP = hp;

            this.AddEffect -= this.BaseAddEffect;
        }
    }
}
