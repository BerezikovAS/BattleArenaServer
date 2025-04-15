using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Debuffs
{
    public class GreateJudgementDebuff : Effect
    {
        public GreateJudgementDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "GreateJudgement";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = "Весь входящий урон конвертируется в чистый";

            effectTags.Add(Consts.EffectTag.GreateJudgement);
        }

        public override void ApplyEffect(Hero _hero)
        {

        }

        public override void RemoveEffect(Hero _hero)
        {

        }
    }
}
