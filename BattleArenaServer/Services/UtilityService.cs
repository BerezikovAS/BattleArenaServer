﻿using BattleArenaServer.Models;

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

        public static List<Hex> GetHexesCircle(Hex casterHex, Hex targetHex, int radius)
        {
            List<Hex> hexesRadius = new List<Hex>();
            if (casterHex != null && targetHex != null && casterHex.Distance(targetHex) <= radius)
            {
                int dist = casterHex.Distance(targetHex);
                foreach (var n in GameData._hexes)
                {
                    if (n.Distance(casterHex) == dist)
                        hexesRadius.Add(n);
                }
            }
            return hexesRadius;
        }

        public static List<Hex> GetHexesLines(Hex casterHex, int _radius = 100)
        {
            List<Hex> hexesLines = new List<Hex>();
            if (casterHex != null)
            {
                foreach (var n in GameData._hexes)
                {
                    if (n.Distance(casterHex) <= _radius & IsOnLine(casterHex, n))
                        hexesLines.Add(n);
                }
            }
            return hexesLines;
        }

        public static List<Hex> GetHexesCone(Hex caster, Hex target, int radius)
        {
            List<Hex> hexesCone = new List<Hex>();

            //Найдем три направления
            if (caster.Distance(target) <= radius && IsOnLine(caster, target))
            {
                //Находим направление ветки по прямой от нас
                Hex dirFront = GetDirection(caster, target);

                //Находим куда двигаться по левой ветке.
                Hex dirLeft = Consts.directions.FirstOrDefault(x => x.IsThisCoord(dirFront.COORD));
                int indDirLeft = Consts.directions.IndexOf(dirLeft) - 1 < 0 ? 5 : Consts.directions.IndexOf(dirLeft) - 1;
                dirLeft = Consts.directions[indDirLeft];

                //Находим куда двигаться по правой ветке.
                Hex dirRight = Consts.directions.FirstOrDefault(x => x.IsThisCoord(dirFront.COORD));
                int indDirRight= Consts.directions.IndexOf(dirRight) + 1 > 5 ? 0 : Consts.directions.IndexOf(dirRight) + 1;
                dirRight = Consts.directions[indDirRight];

                Hex dirClockwise1 = GetDirection(dirLeft, dirFront);
                Hex dirClockwise2 = GetDirection(dirFront, dirRight);

                for (int i = 1; i <= radius; i++)
                {
                    int[] helper = AddDirection(caster.COORD, dirRight.COORD, i);
                    Hex? hexHelper = GameData._hexes.FirstOrDefault(x => x.IsThisCoord(helper));
                    if (hexHelper != null)
                        hexesCone.Add(hexHelper);

                    //клоквайсы могут не попадать в поле, из-за чего становятся нуллами!!!!
                    int[] helper1 = AddDirection(caster.COORD, dirLeft.COORD, i);
                    Hex? clocwise1 = GameData._hexes.FirstOrDefault(x => x.IsThisCoord(helper1));
                    if (clocwise1 != null)
                        hexesCone.Add(clocwise1);

                    int[] helper2 = AddDirection(caster.COORD, dirFront.COORD, i);
                    Hex? clocwise2 = GameData._hexes.FirstOrDefault(x => x.IsThisCoord(helper2));
                    if (clocwise2 != null)
                        hexesCone.Add(clocwise2);

                    for (int j = 1; j < i; j++)
                    {
                        Hex? h1 = GameData._hexes.FirstOrDefault(x => x.IsThisCoord(AddDirection(helper1, dirClockwise1.COORD, j)));
                        if (h1 != null)
                            hexesCone.Add(h1);
                        Hex? h2 = GameData._hexes.FirstOrDefault(x => x.IsThisCoord(AddDirection(helper2, dirClockwise2.COORD, j)));
                        if (h2 != null)
                            hexesCone.Add(h2);
                    }
                }
            }
            
            return hexesCone;
        }

        public static List<Hex> GetHexesSmallCone(Hex caster, Hex target, int radius)
        {
            List<Hex> hexesCone = new List<Hex>();

            //Найдем два направления
            if (caster.Distance(target) <= radius && IsOnLine(caster, target))
            {
                //Находим направление ветки по прямой от нас
                Hex dirFront = GetDirection(caster, target);

                //Находим куда двигаться по правой ветке.
                Hex dirRight = Consts.directions.FirstOrDefault(x => x.IsThisCoord(dirFront.COORD));
                int indDirRight = Consts.directions.IndexOf(dirRight) + 1 > 5 ? 0 : Consts.directions.IndexOf(dirRight) + 1;
                dirRight = Consts.directions[indDirRight];

                Hex dirClockwise2 = GetDirection(dirFront, dirRight);

                for (int i = 1; i <= radius; i++)
                {
                    int[] helper = AddDirection(caster.COORD, dirRight.COORD, i);
                    Hex? hexHelper = GameData._hexes.FirstOrDefault(x => x.IsThisCoord(helper));
                    if (hexHelper != null)
                        hexesCone.Add(hexHelper);

                    //клоквайсы могут не попадать в поле, из-за чего становятся нуллами!!!!

                    int[] helper2 = AddDirection(caster.COORD, dirFront.COORD, i);
                    Hex? clocwise2 = GameData._hexes.FirstOrDefault(x => x.IsThisCoord(helper2));
                    if (clocwise2 != null)
                        hexesCone.Add(clocwise2);

                    for (int j = 1; j < i; j++)
                    {
                        Hex? h2 = GameData._hexes.FirstOrDefault(x => x.IsThisCoord(AddDirection(helper2, dirClockwise2.COORD, j)));
                        if (h2 != null)
                            hexesCone.Add(h2);
                    }
                }
            }

            return hexesCone;
        }

        public static List<Hex> GetHexesWideLine(Hex caster, Hex target, int radius)
        {
            List<Hex> hexesLines = new List<Hex>();
            if (caster != null && target != null)
            {
                if (IsOnLine(caster, target))
                {
                    Hex direction = GetDirection(caster, target);

                    //Находим рядомстоящий гекс по прямой и соседние от нас и него. Это будут начала боковых линий
                    Hex? frontHex = GetOneHexOnDirection(caster, target, 1);
                    if (frontHex == null)
                        return hexesLines;

                    List<Hex> adjustHexes = GameData._hexes.FindAll(x => x.Distance(frontHex) == 1 && x.Distance(caster) == 1);
                    adjustHexes.Add(frontHex);

                    foreach (Hex hex in adjustHexes)
                    {
                        bool endOfField = false;
                        int counter = 0;

                        while (counter < radius && !endOfField)
                        {
                            Hex? findHex = GameData._hexes.FirstOrDefault(x => x.COORD[0] == hex.COORD[0] + direction.COORD[0] * counter
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
            }
            return hexesLines;
        }

        public static Hex? GetRandomAdjacentHex(Hex targetHex)
        {
            List<Hex> freeHexes = new List<Hex>();

            foreach (Hex hex in GetHexesRadius(targetHex, 1))
            {
                if (hex.IsFree())
                    freeHexes.Add(hex);
            }

            if (freeHexes.Count == 0)
                return null;
            Random rnd = new Random();
            return freeHexes[rnd.Next(0, freeHexes.Count)];
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
            Hex direction = new Hex(coordX, coordY, coordZ, -1, 0);
            return direction;
        }

        public static Hex? GetOneHexOnDirection(Hex hex, Hex dirHex, int order, int mode = 0)
        {
            Hex? direction = null;
            if (mode == 1)
                direction = dirHex;
            else
                direction = GetDirection(hex, dirHex);

            Hex ? findHex = null;

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

        public static int[] AddDirection(int[] coord1, int[] coord2, int dist)
        {
            int[] res = new int[coord1.Length];
            res[0] = coord1[0];
            res[1] = coord1[1];
            res[2] = coord1[2];

            res[0] += coord2[0] * dist;
            res[1] += coord2[1] * dist;
            res[2] += coord2[2] * dist;
            return res;
        }

        public static List<Hero> GetBanchHeroes()
        {
            List<Hero> list = new List<Hero>();

            foreach (var hero in GameData._heroes)
            {
                if (hero.IsMainHero && hero.Team == GameData.activeTeam)
                    list.Add(hero);
            }

            return list;
        }
    }
}
