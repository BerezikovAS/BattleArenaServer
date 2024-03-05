using BattleArenaServer.Models;
using System.Text.RegularExpressions;

namespace BattleArenaServer.Services
{
    public class UtilityService
    {
        public UtilityService() { }

        public List<Hex> GetHexesRadius(List<Hex> _hexes, int _idhex, int _radius)
        {
            List<Hex> hexesRadius = new List<Hex>();
            Hex? hex = _hexes.FirstOrDefault(x => x.ID == _idhex);
            if(hex != null)
            {
                foreach (var n in _hexes)
                {
                    if(n.Distance(hex) <= _radius)
                        hexesRadius.Add(n);
                }
            }
            return hexesRadius;
        }

        public List<Hex> GetHexesLines(List<Hex> _hexes, int _idhex, int _radius = 100)
        {
            List<Hex> hexesLines = new List<Hex>();
            Hex? hex = _hexes.FirstOrDefault(x => x.ID == _idhex);
            if (hex != null)
            {
                foreach (var n in _hexes)
                {
                    if (n.Distance(hex) <= _radius & IsOnLine(hex, n))
                        hexesLines.Add(n);
                }
            }
            return hexesLines;
        }

        public List<Hex> GetHexesOneLine(List<Hex> _hexes, int _idhex, int _direction, int _radius = 100)
        {
            List<Hex> hexesLines = new List<Hex>();
            Hex? hex = _hexes.FirstOrDefault(x => x.ID == _idhex);
            Hex? dir = _hexes.FirstOrDefault(x => x.ID == _direction);
            if (hex != null & dir != null)
            {
                if (IsOnLine(hex, dir))
                {
                    Hex direction = GetDirection(hex, dir);
                    bool endOfField = false;
                    int counter = 1;

                    while(hexesLines.Count < _radius && !endOfField)
                    {
                        Hex? findHex = _hexes.FirstOrDefault(x => x.COORD[0] == hex.COORD[0] + direction.COORD[0] * counter
                            & x.COORD[1] == hex.COORD[1] + direction.COORD[1] * counter
                            & x.COORD[2] == hex.COORD[2] + direction.COORD[2] * counter);
                        if (findHex != null)
                            hexesLines.Add(findHex);
                        else
                            endOfField = true;
                        counter++;
                    }
                }
            }
            //TODO починить
            return hexesLines;
        }

        public bool IsOnLine(Hex h1, Hex h2)
        {
            if (h1.Equals(h2))
                return false;

            for (int i = 0; i < 3; i++) 
            {
                if (h1.COORD[i] == h2.COORD[i])
                    return true;
            }
            return false;
        }

        public Hex GetDirection(Hex h1, Hex h2)
        {
            int dist = h1.Distance(h2);
            int coordX = (h2.COORD[0] - h1.COORD[0]) / dist;
            int coordY = (h2.COORD[1] - h1.COORD[1]) / dist;
            int coordZ = (h2.COORD[2] - h1.COORD[2]) / dist;
            Hex direction = new Hex(coordX, coordY, coordZ, -1);
            return direction;
        }

    }
}
