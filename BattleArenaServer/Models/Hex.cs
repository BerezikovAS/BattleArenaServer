using BattleArenaServer.Models.Obstacles;

namespace BattleArenaServer.Models
{
    public class Hex
    {
        int _id = 0;
        int[] _coord = new int[3];
        Hero? Hero { get; set; }
        Obstacle? Obstacle { get; set; }
        List<FillableObstacle> Surfaces { get; set; } = new List<FillableObstacle>();

        public Hex(int[] coord)
        {
            _coord = coord;
            Hero = null;
            _id = 0;
        }

        public Hex(int coordX, int coordY, int coordZ, int id)
        {
            _coord = new int[3] { coordX, coordY, coordZ };
            Hero = null;
            _id = id;
        }

        public int[] COORD { get { return _coord; } }

        public Hero? HERO { get { return Hero; } }

        public Obstacle? OBSTACLE { get { return Obstacle; } }

        public List<FillableObstacle> SURFACES { get { return Surfaces; } }

        public int ID { get { return _id; } }

        public void SetHero(Hero hero)
        {
            Hero = hero;
            Hero.HexId = _id;
        }

        public void RemoveHero()
        {
            Hero = null;
        }

        public void SetObstacle(Obstacle obstacle)
        {
            Obstacle = obstacle;
            obstacle.HexId = _id;
            GameData._obstacles.Add(obstacle);
        }

        public void RemoveObstacle()
        {
            Obstacle = null;
            GameData._obstacles.Remove(Obstacle);
        }

        public void AddSurface(FillableObstacle surface)
        {
            Surfaces.Add(surface);
            GameData._surfaces.Add(surface);
        }

        public void RemoveSurface(FillableObstacle surface)
        {
            Surfaces.Remove(surface);
        }

        public bool IsFree()
        {
            return Hero == null;
        }

        public int Distance(Hex hex)
        {
            int[] x = new int[3];
            x[0] = _coord[0];
            x[1] = _coord[1];
            x[2] = _coord[2];

            int[] y = new int[3];
            y[0] = hex.COORD[0];
            y[1] = hex.COORD[1];
            y[2] = hex.COORD[2];

            x[0] -= y[0];
            x[1] -= y[1];
            x[2] -= y[2];

            int d = 0;
            for (int i = 0; i < 3; i++)
            {
                if (Math.Abs(x[i]) >= d)
                    d = Math.Abs(x[i]);
            }
            return d;
        }

        public bool IsThisCoord(int[] coord)
        {
            if (_coord[0] == coord[0] && _coord[1] == coord[1] && _coord[2] == coord[2])
                return true;
            return false;
        }
    }
}
