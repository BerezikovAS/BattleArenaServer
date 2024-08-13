using BattleArenaServer.Models;

namespace BattleArenaServer.Services
{
    public static class UtilityService
    {
        public static List<Hex> GetHexesRadius(Hex targetHex, int radius)
        {
            List<Hex> hexesRadius = new List<Hex>();
            if (targetHex != null)
            {
                foreach (var n in GameData._hexes)
                {
                    if (n.Distance(targetHex) <= radius)
                        hexesRadius.Add(n);
                }
            }
            return hexesRadius;
        }

        public static List<Hex> GetHexesLines(List<Hex> _hexes, int _idhex, int _radius = 100)
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

        public static List<Hex> GetHexesCone(Hex caster, Hex target, int radius)
        {
            List<Hex> hexesLines = new List<Hex>();

            //Найдем три направления

            
            return hexesLines;
        }

        public static List<Hex> GetHexesOneLine(Hex caster, Hex target, int radius = 100)
        {
            List<Hex> hexesLines = new List<Hex>();
            if (caster != null && target != null)
            {
                if (IsOnLine(caster, target))
                {
                    Hex direction = GetDirection(caster, target);
                    bool endOfField = false;
                    int counter = 1;

                    while (hexesLines.Count < radius && !endOfField)
                    {
                        Hex? findHex = GameData._hexes.FirstOrDefault(x => x.COORD[0] == caster.COORD[0] + direction.COORD[0] * counter
                            & x.COORD[1] == caster.COORD[1] + direction.COORD[1] * counter
                            & x.COORD[2] == caster.COORD[2] + direction.COORD[2] * counter);
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

        public static bool IsOnLine(Hex h1, Hex h2)
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

        public static Hex GetDirection(Hex h1, Hex h2)
        {
            int dist = h1.Distance(h2);
            int coordX = (h2.COORD[0] - h1.COORD[0]) / dist;
            int coordY = (h2.COORD[1] - h1.COORD[1]) / dist;
            int coordZ = (h2.COORD[2] - h1.COORD[2]) / dist;
            Hex direction = new Hex(coordX, coordY, coordZ, -1);
            return direction;
        }

        public static Hex? GetOneHexOnDirection(Hex hex, Hex dir, int order)
        {
            Hex direction = GetDirection(hex, dir);
            Hex? findHex = null;

            for (int i = 1; i <= order; i++)
            {
                findHex = GameData._hexes.FirstOrDefault(x => x.COORD[0] == hex.COORD[0] + direction.COORD[0] * i
                                & x.COORD[1] == hex.COORD[1] + direction.COORD[1] * i
                                & x.COORD[2] == hex.COORD[2] + direction.COORD[2] * i);
            }
            if (findHex != null)
                return findHex;
            return null;
        }
    }
}
