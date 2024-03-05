using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Models.Heroes;

namespace BattleArenaServer.Services
{
    public class FieldService : IField
    {
        //private readonly ITiming _timing;

        List<Hex> hexes = new List<Hex>();
        List<Hero> heroes = new List<Hero>();
        UtilityService util = new UtilityService();

        public FieldService() 
        {
            //_timing = timing;

            int[] _start = new int[3]{ -1, -3, 4 };
            for (int i = 0; i < 52; i++)
            {
                Hex hex = new Hex(_start[0], _start[1], _start[2], i);
                hexes.Add(hex);
                switch (i) 
                {
                    case 6: _start = new int[3] { -2, -2, 4 }; break;
                    case 14: _start = new int[3] { -2, -1, 3 }; break;
                    case 21: _start = new int[3] { -3, 0, 3 }; break;
                    case 29: _start = new int[3] { -3, 1, 2 }; break;
                    case 36: _start = new int[3] { -4, 2, 2 }; break;
                    case 44: _start = new int[3] { -4, 3, 1 }; break;
                    default:
                    {
                        _start[0] += Consts.directions[1].COORD[0];
                        _start[1] += Consts.directions[1].COORD[1];
                        _start[2] += Consts.directions[1].COORD[2];
                        break;
                    }
                }
            }
            Hero hero1 = new Knight();
            Hero hero2 = new Angel();
            Hero hero3 = new Berserker();
            Hero hero4 = new Priest();
            hexes[0].setHero(hero1);
            hexes[1].setHero(hero2);
            hexes[7].setHero(hero3);
            hexes[8].setHero(hero4);

            heroes.Add(hero1);
            heroes.Add(hero2);
            heroes.Add(hero3);
            heroes.Add(hero4);
        }

        public List<Hex> GetField()
        {
            return hexes;
        }

        public List<Hero> GetHeroes()
        {
            return heroes;
        }

        public List<Hex> StepHero(int _cur_pos, int _targer_pos)
        {
            Hex? hexCurrent = hexes.FirstOrDefault(x => x.ID == _cur_pos);
            Hex? hexTarget = hexes.FirstOrDefault(x => x.ID == _targer_pos & x.HERO == null);

            if (hexCurrent != null & hexTarget != null)
            {
                if (hexCurrent.Distance(hexTarget) != 1 | hexCurrent.HERO.AP < 1)
                    return new List<Hex>();

                hexCurrent.HERO.AP -= 1;
                hexTarget.setHero(hexCurrent.HERO);
                hexCurrent.setHero(null);
            }
            return hexes;
        }

        public List<Hex> AttackHero(int _cur_pos, int _targer_pos)
        {
            Hex? hexCurrent = hexes.FirstOrDefault(x => x.ID == _cur_pos);
            Hex? hexTarget = hexes.FirstOrDefault(x => x.ID == _targer_pos & x.HERO != null);

            if (hexCurrent != null & hexTarget != null)
            {
                if (hexCurrent.Distance(hexTarget) > hexCurrent.HERO.AttackRadius | hexCurrent.HERO.AP < 2)
                    return new List<Hex>();

                hexCurrent.HERO.AP -= 2;
                hexTarget.SetDamage(hexCurrent.HERO.Dmg, "phys");
            }

            return hexes;
        }

        public bool SpellCast(int _target, int _caster, int _spell)
        {
            Hero hero = hexes.FirstOrDefault(x => x.ID == _caster).HERO;
            if (hero != null)
            {
                return hero.SkillList[_spell - 1].Cast(hexes, _target, _caster);
            }
            return false;
        }

        public List<Hex> SpellArea(int _target, int _caster, int _spell)
        {
            //Hero hero = hexes.FirstOrDefault(x => x.ID == _caster).HERO;
            //if (hero != null)
            //{
            //    return hero.SkillList[_spell - 1].SpellArea(hexes, _target, _caster);
            //}
            return new List<Hex>();
        }
    }
}
