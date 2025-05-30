﻿using BattleArenaServer.Models;

namespace BattleArenaServer
{
    public interface IField
    {
        public List<Hex> GetField();

        public List<Hero> GetHeroes();

        public List<Hex> StepHero(int _cur_pos, int _targer_pos);

        public List<Hex> AttackHero(int _cur_pos, int _targer_pos);

        public bool SpellCast(int _target, int _caster, int _spell);

        public bool UpgradeSkill(int _cur_pos, int _skill);

        //public int[] GetSpellArea(int target, int caster, int spell);
    }
}
