using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Debuffs
{
    public class BlindDebuff : Effect
    {
        int prevRange = 0;
        public BlindDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "Blind";
            type = "debuff";
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
        }

        public override void ApplyEffect(Hero _hero)
        {
            prevRange = _hero.AttackRadius;
            _hero.AttackRadius = 1;

            foreach (var skill in _hero.SkillList)
            {
                skill.range = 1;
            }
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.AttackRadius = prevRange;
            foreach (var skill in _hero.SkillList)
            {
                skill.range = skill.stats.range;
            }
        }
    }
}
