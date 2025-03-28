using BattleArenaServer.Hubs;
using BattleArenaServer.Models;
using BattleArenaServer.Models.Heroes;

namespace BattleArenaServer.Services
{
    public class FieldService : IField
    {
        public FieldService()
        {
            CreateGameField();
            CreateHeroList();
        }

        private static void CreateHeroList()
        {
            SetRandomCommands();
            // Применяем эффекты постоянных аур сразу
            AttackService.ContinuousAuraAction();
        }

        private static void SetRandomCommands()
        {
            List<Hero> heroes = new List<Hero>();
            List<int> redCoords = [7, 22, 37];
            List<int> blueCoords = [14, 29, 44];

            heroes.Add(new KnightHero(0, ""));
            heroes.Add(new ArcherHero(0, ""));
            heroes.Add(new BerserkerHero(0, ""));
            heroes.Add(new PriestHero(0, ""));
            heroes.Add(new AeroturgHero(0, ""));
            heroes.Add(new GeomantHero(0, ""));
            heroes.Add(new AbominationHero(0, ""));
            heroes.Add(new ChaosHero(0, ""));
            heroes.Add(new ElementalistHero(0, ""));
            heroes.Add(new CultistHero(0, ""));

            string team = "red";
            Random rnd = new Random();
            int rndCoords = 0;

            for (int i = 0; i < 6; i++)
            {
                Hero hero = GetRandomHero(heroes);
                heroes.Remove(hero);
                hero.Id = i;
                hero.Team = team;                

                if (team == "red")
                {
                    rndCoords = rnd.Next(redCoords.Count);
                    hero.HexId = redCoords[rndCoords];
                    redCoords.Remove(redCoords[rndCoords]);
                    team = "blue";
                }
                else
                {
                    rndCoords = rnd.Next(blueCoords.Count);
                    hero.HexId = blueCoords[rndCoords];
                    blueCoords.Remove(blueCoords[rndCoords]);
                    team = "red";
                }
                GameData._hexes[hero.HexId].SetHero(hero);
                GameData._heroes.Add(hero);
            }
        }

        private static Hero GetRandomHero(List<Hero> heroes)
        {
            Random rnd = new Random();
            Hero hero = heroes[rnd.Next(0, heroes.Count)];
            heroes.Remove(hero);

            return hero;
        }

        private void CreateGameField()
        {
            // Создание поля
            int[] _start = [-1, -3, 4];
            for (int i = 0; i < 52; i++)
            {
                Hex hex = new Hex(_start[0], _start[1], _start[2], i);
                GameData._hexes.Add(hex);
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

        public List<Hex> GetField()
        {
            //fieldHub.SendField();
            return GameData._hexes;
        }

        public List<Hero> GetHeroes()
        {
            return GameData._heroes;
        }

        public bool UpgradeSkill(int cur_pos, int skill)
        {
            Hero? hero = GameData._hexes.FirstOrDefault(x => x.ID == cur_pos)?.HERO;
            if (hero != null)
            {
                // Пытаемся прокачать скилл
                if (hero.SkillList[skill].UpgradeSkill())
                {
                    hero.UpgradePoints--;
                    return true;
                }
            }
            return false;
        }

        public List<Hex> StepHero(int cur_pos, int targer_pos)
        {
            RequestData requestData = new RequestData(cur_pos, targer_pos);
            if (requestData.Caster != null)
                requestData.Caster.MoveSkill.Cast(requestData);

            //fieldHub.SendField();
            return GameData._hexes;
        }

        public List<Hex> AttackHero(int cur_pos, int targer_pos)
        {
            RequestData requestData = new RequestData(cur_pos, targer_pos);

            if (requestData.CasterHex != null && requestData.TargetHex != null && requestData.Caster != null && requestData.Target != null)
            {
                Hero attacker = requestData.Caster;
                Hero defender = requestData.Target;

                int range = attacker.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Blind)) == null ? attacker.AttackRadius + attacker.StatsEffect.AttackRadius : 1;

                if (requestData.CasterHex.Distance(requestData.TargetHex) > range || attacker.AP < attacker.APtoAttack)
                    return new List<Hex>();

                attacker.AP -= attacker.APtoAttack;
                // К урону добавляем дополнительный от пассивок и эффектов
                int dmg = attacker.Dmg + attacker.GetPassiveAttackDamage(attacker, defender) + attacker.StatsEffect.Dmg;

                // Эффекты перед атакой
                attacker.beforeAttack(attacker, defender, dmg);
                // Сама атака с нанесением урона
                AttackService.SetDamage(attacker, defender, dmg, Consts.DamageType.Physical);
                // Эффекты после атаки
                attacker.afterAttack(attacker, defender, dmg);
            }

            return GameData._hexes;
        }

        public bool SpellCast(int targer_pos, int cur_pos, int skill)
        {
            RequestData requestData = new RequestData(cur_pos, targer_pos);
            if (requestData.Caster != null && requestData.Caster.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Silence)) == null)
            {
                requestData.Caster.beforeSpellCast(requestData.Caster, requestData.Target, requestData.Caster.SkillList[skill]);
                // Кастуем))
                return requestData.Caster.SkillList[skill].Cast(requestData);
            }
            return false;
        }

       
    }
}
