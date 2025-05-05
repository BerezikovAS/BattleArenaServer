using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Buffs
{
    public class SpiritLinkBuff : Effect
    {
        public SpiritLinkBuff(int _idCaster, int _value, int _duration, string _targetHeroName)
        {
            Name = "SpiritLink";
            type = Consts.StatusEffect.Buff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = $"Ваши ОД используются совместно с {_targetHeroName}";
            effectTags.Add(Consts.EffectTag.SpiritLink);
        }

        public override void ApplyEffect(Hero _hero)
        {
            _hero.SpendAP -= _hero.BaseSpendAP;
            _hero.SpendAP += _hero.SpiritLinkSpendAP;
        }

        public override void RemoveEffect(Hero _hero)
        {
            // Связь разорвалась. Убираем её и со связанного героя
            Hero? caster = GameData._heroes.FirstOrDefault(x => x.Id == idCaster);
            if (caster != null)
            {
                Effect? spiritLink = caster.EffectList.FirstOrDefault(x => x.Name == Name && x.idCaster == _hero.Id);
                if (spiritLink != null)
                {
                    _hero.SpendAP -= _hero.SpiritLinkSpendAP;
                    _hero.SpendAP += _hero.BaseSpendAP;

                    caster.EffectList.Remove(spiritLink);
                    spiritLink.RemoveEffect(caster);
                }
            }
        }
    }
}
