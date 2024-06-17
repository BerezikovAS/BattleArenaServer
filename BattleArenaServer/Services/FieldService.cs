using BattleArenaServer.Interfaces;
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
            // Создание и размещение героев на поле. (Для тестов)
            Hero hero1 = new KnightHero();
            Hero hero2 = new CrossbowmanHero();
            Hero hero3 = new BerserkerHero();
            Hero hero4 = new PriestHero();
            Hero hero5 = new AeroturgHero();
            Hero hero6 = new GeomantHero();

            GameData._hexes[7].SetHero(hero1);
            GameData._hexes[22].SetHero(hero2);
            GameData._hexes[37].SetHero(hero6);
            GameData._hexes[29].SetHero(hero3);
            GameData._hexes[14].SetHero(hero4);
            GameData._hexes[44].SetHero(hero5);

            GameData._heroes.Add(hero1);
            GameData._heroes.Add(hero2);
            GameData._heroes.Add(hero3);
            GameData._heroes.Add(hero4);
            GameData._heroes.Add(hero5);
            GameData._heroes.Add(hero6);

            // Применяем эффекты постоянных аур сразу
            AttackService.ContinuousAuraAction();
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
                if (hero.SkillList[skill - 1].UpgradeSkill())
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
            return GameData._hexes;
        }

        public List<Hex> AttackHero(int cur_pos, int targer_pos)
        {
            RequestData requestData = new RequestData(cur_pos, targer_pos);

            if (requestData.CasterHex != null && requestData.TargetHex != null && requestData.Caster != null && requestData.Target != null)
            {
                Hero attacker = requestData.Caster;
                Hero defender = requestData.Target;

                int range = attacker.EffectList.FirstOrDefault(x => x.Name == "Blind") == null ? attacker.AttackRadius + attacker.StatsEffect.AttackRadius : 1;

                if (requestData.CasterHex.Distance(requestData.TargetHex) > range || attacker.AP < attacker.APtoAttack)
                    return new List<Hex>();

                attacker.AP -= attacker.APtoAttack;
                // К урону добавляем дополнительный от пассивок и эффектов
                int dmg = attacker.Dmg + attacker.passiveAttackDamage(attacker, defender) + attacker.StatsEffect.Dmg;

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
            if (requestData.Caster != null && requestData.Caster.EffectList.FirstOrDefault(x => x.Name == "Silence") == null)
            {
                // Кастуем))
                return requestData.Caster.SkillList[skill - 1].Cast(requestData);
            }
            return false;
        }
    }
}
