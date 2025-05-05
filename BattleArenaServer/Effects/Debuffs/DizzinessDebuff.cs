using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Debuffs
{
    public class DizzinessDebuff : Effect
    {
        public DizzinessDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "Dizziness";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = $"Перемещаясь своим ходом, Вы можете случайно попасть на соседний гекс.";

            effectTags.Add(Consts.EffectTag.Dizziness);
        }

        public override void ApplyEffect(Hero hero)
        {

        }

        public override void RemoveEffect(Hero hero)
        {

        }
    }
}
