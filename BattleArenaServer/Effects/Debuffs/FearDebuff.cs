using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Debuffs
{
    public class FearDebuff : Effect
    {
        public FearDebuff(int _idCaster, int _value, int _duration, string _casterName)
        {
            Name = "Fear";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = $"Действие перемещения доступно только в сторону от {_casterName}.";

            effectTags.Add(Consts.EffectTag.Fear);
        }

        public override void ApplyEffect(Hero hero)
        {

        }

        public override void RemoveEffect(Hero hero)
        {

        }
    }
}
