using BattleArenaServer.Models.Obstacles;

namespace BattleArenaServer.Models
{
    public static class GameData
    {
        public static int idActiveHero;

        public static string activeTeam;

        public static UserTeamBindings userTeamBindings = new UserTeamBindings();

        public static List<Hex> _hexes = new List<Hex>();

        public static List<Hero> _heroes = new List<Hero>();

        public static List<Obstacle> _obstacles = new List<Obstacle>();

        public static List<SolidObstacle> _solidObstacles = new List<SolidObstacle>();

        public static List<FillableObstacle> _surfaces = new List<FillableObstacle>();

        public static List<Item> _blueShop = new List<Item>();

        public static List<Item> _redShop = new List<Item>();

        public static void ClearAllObjects()
        {
            activeTeam = "red";
            _heroes.Clear();
            _hexes.Clear();
            _obstacles.Clear();
            _surfaces.Clear();
            _solidObstacles.Clear();
        }

    }
}
