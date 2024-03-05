namespace BattleArenaServer.Models
{
    public class Hex
    {
        int id = 0;
        int[] coord = new int[3];
        Hero? Hero { get; set; }

        public Hex(int[] _coord) 
        {
            coord = _coord;
            Hero = null;
            id = 0;
        }

        public Hex(int _coordX, int _coordY, int _coordZ, int _id)
        {
            coord = new int[3] {_coordX, _coordY, _coordZ};
            Hero = null;
            id = _id;
        }

        public int[] COORD { get { return coord; } }

        public Hero? HERO {  get { return Hero; } }

        public int ID { get { return id; } }

        public void setHero( Hero _hero )
        {
            Hero = _hero;
        }

        public void removeHero(bool _remove)
        {
            if (_remove)
                Hero = null;
        }

        public bool SetDamage(int _dmg, string _typeDmg)
        {
            if(Hero != null)
            {
                double totalDmg = 0;
                switch (_typeDmg)
                {
                    case "phys":
                        {
                            int armor = Hero.Armor;
                            totalDmg = _dmg * (1 - (0.1 * armor) / (1 + 0.1 * armor));
                        }
                        break;
                    case "magic":
                        {
                            int resist = Hero.Resist;
                            totalDmg = _dmg * (1 - (0.1 * resist) / (1 + 0.1 * resist));
                        }
                        break;
                    case "pure":
                        {
                            totalDmg = _dmg;
                        }
                        break;
                }

                Hero.HP -= ((int)totalDmg);

                if (Hero.HP <= 0)
                {
                    Hero = null;
                    return true;
                }
                return false;
            }
            return false;
        }

        public int Distance(Hex _hex)
        {
            int[] x = new int[3];
            x[0] = coord[0];
            x[1] = coord[1];
            x[2] = coord[2];

            int[] y = new int[3];
            y[0] = _hex.COORD[0];
            y[1] = _hex.COORD[1];
            y[2] = _hex.COORD[2];

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
    }
}
