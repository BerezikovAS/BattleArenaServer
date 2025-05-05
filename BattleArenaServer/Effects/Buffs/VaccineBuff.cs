using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Buffs
{
    public class VaccineBuff : Effect
    {
        int extraHeal = 0;
        public VaccineBuff(int _idCaster, int _value, int _duration, int _extraHeal)
        {
            Name = "Vaccine";
            type = Consts.StatusEffect.Buff;
            idCaster = _idCaster;
            value = _value;
            extraHeal = _extraHeal;
            duration = _duration;
            effectType = Consts.EffectType.StartTurn;
            description = $"Восстанавливает {value} ХП + {extraHeal} ХП за каждый дебафф в начале своего хода. Также убирает один случайный дебафф.";
        }

        public override void ApplyEffect(Hero hero)
        {
            List<Effect> effects = new List<Effect>();
            effects = hero.EffectList.FindAll(x => x.type == Consts.StatusEffect.Debuff);

            int totalHeal = value + effects.Count * extraHeal;
            hero.Heal(totalHeal);

            if (effects.Count > 0)
            {
                Random rnd = new Random();
                Effect? effect = effects[rnd.Next(effects.Count)];
                if (effect != null)
                {
                    effect.RemoveEffect(hero);
                    hero.EffectList.Remove(effect);
                }
            }
        }

        public override void RemoveEffect(Hero hero)
        {

        }
    }
}
