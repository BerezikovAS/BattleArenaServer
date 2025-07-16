using BattleArenaServer.Interfaces;
using BattleArenaServer.Models.Obstacles;
using BattleArenaServer.Services;

namespace BattleArenaServer.Models
{
    public class GameData : IGameData
    {
        public GameData()
        {
            CreateGameField();
            pickService = new PickService(this);
            pickService.FillHeroesList();
            timingService = new TimingService(this);
            shopService = new ShopService(this);
            shopService.FillItems();
        }

        public PickService pickService;
        public TimingService timingService;
        public ShopService shopService;

        

        public int idActiveHero;

        public int turn = 1;

        public string activeTeam = "red";

        public UserTeamBindings userTeamBindings = new UserTeamBindings();

        public List<Hex> _hexes = new List<Hex>();

        public List<Hero> _heroes = new List<Hero>();

        public List<Obstacle> _obstacles = new List<Obstacle>();

        public List<SolidObstacle> _solidObstacles = new List<SolidObstacle>();

        public List<FillableObstacle> _surfaces = new List<FillableObstacle>();

        public List<Item> _blueShop = new List<Item>();

        public List<Item> _redShop = new List<Item>();

        public List<ChatMessage> messages = new List<ChatMessage>();

        public void ClearAllObjects()
        {
            activeTeam = "red";
            _heroes.Clear();
            _hexes.Clear();
            _obstacles.Clear();
            _surfaces.Clear();
            _solidObstacles.Clear();
        }

        public void RecreateGame()
        {
            ClearAllObjects();
            activeTeam = "red";
            CreateGameField();
            shopService.FillItems();
            timingService = new TimingService(this);
            userTeamBindings.ClearBindings();
            pickService.FillHeroesList(); // Обновляется список героев для пика
        }

        private void CreateGameField()
        {
            // Создание поля
            int[] _start = [-1, -3, 4];
            for (int i = 0; i < 52; i++)
            {
                //Устанавливаем победные очки за контроль гекса
                int vp = 0;
                switch (i)
                {
                    case 10: case 11: case 17: case 19: case 24: case 27: case 32: case 34: case 40: case 41: vp = 1; break;
                    case 18: case 25: case 26: case 33: vp = 2; break;
                    default: vp = 0; break;
                }

                //Устанавливаем точки респавна для каждой из команд
                string teamRespawn = "";
                switch (i)
                {
                    case 0: case 7: case 37: case 45: teamRespawn = "red"; break;
                    case 6: case 14: case 44: case 51: teamRespawn = "blue"; break;
                    default: teamRespawn = ""; break;
                }

                //Устанавливаем зоны покупок предметов для команд
                string teamShop = "";
                switch (i)
                {
                    case 0: case 1: case 7: case 8: case 15: case 22: case 30: case 37: case 38: case 45: case 46: teamShop = "red"; break;
                    case 5: case 6: case 13: case 14: case 21: case 29: case 36: case 43: case 44: case 50: case 51: teamShop = "blue"; break;
                    default: teamShop = ""; break;
                }

                Hex hex = new Hex(_start[0], _start[1], _start[2], i, vp, teamRespawn, teamShop);
                _hexes.Add(hex);

                //Поскольку мы добавляем гексы слева направо, то при достижении определенных значений, нужно "спуститься" на новую строку
                switch (i)
                {
                    case 6: _start = [-2, -2, 4]; break;
                    case 14: _start = [-2, -1, 3]; break;
                    case 21: _start = [-3, 0, 3]; break;
                    case 29: _start = [-3, 1, 2]; break;
                    case 36: _start = [-4, 2, 2]; break;
                    case 44: _start = [-4, 3, 1]; break;
                    default:
                        {
                            _start[0] += Consts.directions[1].COORD[0];
                            _start[1] += Consts.directions[1].COORD[1];
                            _start[2] += Consts.directions[1].COORD[2];
                            break;
                        }
                }
            }
        }
    }
}
