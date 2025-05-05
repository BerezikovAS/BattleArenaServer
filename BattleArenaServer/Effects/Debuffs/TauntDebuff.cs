using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Debuffs
{
    public class TauntDebuff : Effect
    {
        public TauntDebuff(int _idCaster, int _value, int _duration, string _casterName, bool _nonItem = false)
        {
            Name = "Taunt";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = $"Вы можете атаковать только {_casterName}";
            effectTags.Add(Consts.EffectTag.Silence);
            effectTags.Add(Consts.EffectTag.Taunt);

            if (_nonItem)
                effectTags.Add(Consts.EffectTag.NonItem);
        }

        public override void ApplyEffect(Hero _hero)
        {
            Hero? caster = GameData._heroes.FirstOrDefault(x => x.Id == idCaster);
            if (caster != null)
            {
                TauntBuff tauntBuff = new TauntBuff(idCaster, value, duration, _hero.Id, _hero.Name);
                caster.AddEffect(tauntBuff);
            }
        }

        public override void RemoveEffect(Hero _hero)
        {
            Hero? caster = GameData._heroes.FirstOrDefault(x => x.Id == idCaster);
            if (caster != null)
            {
                Effect? taunt = caster.EffectList.FirstOrDefault(x => x.Name == "Taunt" && x.type == Consts.StatusEffect.Buff);
                if (taunt != null)
                {
                    caster.EffectList.Remove(taunt);
                    taunt.RemoveEffect(caster);
                }
            }
        }
    }
}
