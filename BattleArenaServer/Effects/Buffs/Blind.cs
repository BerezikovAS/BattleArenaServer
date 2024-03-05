using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Buffs
{
    public class Blind : Effect
    {
        int prevRange = 0;
        List<int> prevSkillRange = new List<int>();
        public Blind(int _idCaster, int _value, int _duration)
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
                prevSkillRange.Add(skill.range);
                skill.range = 1;
            }
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.AttackRadius = prevRange;
            int i = 0;
            foreach (var skill in _hero.SkillList)
            {
                skill.range = prevSkillRange[i];
                i++;
            }
        }
    }
}
