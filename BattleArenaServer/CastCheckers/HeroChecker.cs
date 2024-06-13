﻿using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.CastCheckers
{
    public class HeroChecker : ICastChecker
    {
        public ICastChecker nextChecker { get; set; } = new TerminalChecker();

        public bool Check(Hero caster, Hero? target, Hex? targetHex, Skill skill)
        {
            if (target != null)
                return nextChecker.Check(caster, target, targetHex, skill);
            return false;
        }
    }
}
