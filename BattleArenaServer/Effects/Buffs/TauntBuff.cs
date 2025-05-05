using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Buffs
{
    public class TauntBuff : Effect
    {
        int target = -1;
        bool isRemoved = false;
        public TauntBuff(int _idCaster, int _value, int _duration, int _target, string _targetName)
        {
            Name = "Taunt";
            type = Consts.StatusEffect.Buff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            target = _target;
            description = $"+{value} брони. Вы спровоцировали {_targetName}";
        }

        public override void ApplyEffect(Hero _hero)
        {
            _hero.Armor += value;
        }

        public override void RemoveEffect(Hero _hero)
        {
            if (!isRemoved)
                _hero.Armor -= value;
            isRemoved = true;

            Hero? targetHero = GameData._heroes.FirstOrDefault(x => x.Id == target);
            if (targetHero != null)
            {
                Effect? taunt = targetHero.EffectList.FirstOrDefault(x => x.Name == "Taunt" && x.type == Consts.StatusEffect.Debuff);
                if (taunt != null)
                {
                    targetHero.EffectList.Remove(taunt);
                    taunt.RemoveEffect(targetHero);
                }
            }
        }
    }
}
