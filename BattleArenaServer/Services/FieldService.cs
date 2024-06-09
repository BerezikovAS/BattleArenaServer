﻿using BattleArenaServer.Models;
using BattleArenaServer.Models.Heroes;

namespace BattleArenaServer.Services
{
    public class FieldService : IField
    {
        public FieldService() 
        {
            // Создание поля
            int[] _start = new int[3]{ -1, -3, 4 };
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

            // Создание и размещение героев на поле. (Для тестов)
            Hero hero1 = new Knight();
            Hero hero2 = new Crossbowman();
            Hero hero3 = new Berserker();
            Hero hero4 = new Priest();
            hero4.Armor = 0;
            GameData._hexes[0].SetHero(hero1);
            GameData._hexes[2].SetHero(hero2);
            GameData._hexes[30].SetHero(hero3);
            GameData._hexes[32].SetHero(hero4);

            GameData._heroes.Add(hero1);
            GameData._heroes.Add(hero2);
            GameData._heroes.Add(hero3);
            GameData._heroes.Add(hero4);

            // Применяем эффекты постоянных аур сразу
            AttackService.ContinuousAuraAction();
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
            Hex? hexCurrent = GameData._hexes.FirstOrDefault(x => x.ID == cur_pos);
            Hex? hexTarget = GameData._hexes.FirstOrDefault(x => x.ID == targer_pos && x.HERO == null);

            if (hexCurrent != null && hexTarget != null && hexCurrent.HERO != null)
            {
                // Перемещаемся строго на одну клеточку. Если стоим в рутах, то двигаться нельзя
                if (hexCurrent.Distance(hexTarget) != 1 || hexCurrent.HERO.AP < 1 || hexCurrent.HERO.EffectList.FirstOrDefault(x => x.Name == "Root") != null)
                    return new List<Hex>();

                hexCurrent.HERO.AP -= 1;
                // Двигаем героя
                AttackService.MoveHero(hexCurrent.HERO, hexCurrent, hexTarget);
            }
            return GameData._hexes;
        }

        public List<Hex> AttackHero(int cur_pos, int targer_pos)
        {
            Hex? hexCurrent = GameData._hexes.FirstOrDefault(x => x.ID == cur_pos);
            Hex? hexTarget = GameData._hexes.FirstOrDefault(x => x.ID == targer_pos && x.HERO != null);

            if (hexCurrent != null && hexTarget != null && hexCurrent.HERO != null && hexTarget.HERO != null)
            {
                Hero attacker = hexCurrent.HERO;
                Hero defender = hexTarget.HERO;
                if (hexCurrent.Distance(hexTarget) > attacker.AttackRadius + attacker.StatsEffect.AttackRadius || attacker.AP < attacker.APtoAttack)
                    return new List<Hex>();

                attacker.AP -= attacker.APtoAttack;
                // К урону добавляем дополнительный от пассивок и эффектов
                int dmg = attacker.Dmg + attacker.passiveAttackDamage(attacker, defender) + attacker.StatsEffect.Dmg;
                AttackService.SetDamage(attacker, defender, dmg, Consts.DamageType.Physical);
                // Применяем эффекты после атаки
                attacker.afterAttack(attacker, defender, dmg);
            }

            return GameData._hexes;
        }

        public bool SpellCast(int targer_pos, int cur_pos, int skill)
        {
            Hero? caster = GameData._hexes.FirstOrDefault(x => x.ID == cur_pos)?.HERO;
            Hero? defender = GameData._hexes.FirstOrDefault(x => x.ID == targer_pos)?.HERO;
            Hex? targetHex = GameData._hexes.FirstOrDefault(x => x.ID == targer_pos);
            if (caster != null)
            {
                // Кастуем))
                return caster.SkillList[skill - 1].Cast(caster, defender, targetHex);
            }
            return false;
        }

        public List<Hex> SpellArea(int _target, int _caster, int _spell)
        {
            return new List<Hex>();
        }
    }
}
