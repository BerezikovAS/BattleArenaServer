using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Debuffs
{
    public class BlindDebuff : Effect
    {
        public BlindDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "Blind";
            type = "debuff";
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
        }

        public override void ApplyEffect(Hero hero)
        {
            hero.StatsEffect.AttackRadius = 1 - hero.AttackRadius;

            foreach (var skill in hero.SkillList)
            {
                skill.range = 1;
            }
        }

        public override void RemoveEffect(Hero hero)
        {
            hero.StatsEffect.AttackRadius = hero.AttackRadius;
            foreach (var skill in hero.SkillList)
            {
                skill.range = skill.stats.range;
            }
        }
    }
}
