using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Debuffs
{
    public class ChainOfPainDebuff : Effect
    {
        public ChainOfPainDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "ChainOfPain";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = $"{value}% полученного урона заклинателем наносится и Вам.";
        }

        public override void ApplyEffect(Hero _hero)
        {
            Hero? caster = GameData._heroes.FirstOrDefault(x => x.Id == idCaster);
            if (caster != null)
            {
                ChainOfPainBuff chainOfPainBuff = new ChainOfPainBuff(idCaster, value, duration, _hero.Id);
                caster.AddEffect(chainOfPainBuff);
                chainOfPainBuff.ApplyEffect(caster);
            }
        }

        public override void RemoveEffect(Hero _hero)
        {
            Hero? caster = GameData._heroes.FirstOrDefault(x => x.Id == idCaster);
            if (caster != null)
            {
                Effect? chainOfPainBuff = caster.EffectList.FirstOrDefault(x => x.Name == "ChainOfPain" && x.type == Consts.StatusEffect.Buff);
                if (chainOfPainBuff != null)
                    caster.EffectList.Remove(chainOfPainBuff);
            }
        }
    }
}
