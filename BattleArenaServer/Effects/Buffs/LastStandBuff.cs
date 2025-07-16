using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Buffs
{
    public class LastStandBuff : Effect
    {
        public LastStandBuff(int _idCaster, int _value, int _duration)
        {
            Name = "LastStand";
            type = Consts.StatusEffect.Buff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = $"Здоровье не может упасть ниже 1. По окончанию действия эффекта восстановит {value} ХП";
        }

        public override void ApplyEffect(Hero _hero, GameData gameData)
        {
            _hero.afterReceiveDmg += AfterReceiveDmgDelegate;
        }

        public override void RemoveEffect(Hero _hero, GameData gameData)
        {
            _hero.afterReceiveDmg -= AfterReceiveDmgDelegate;
        }

        public override void ApplyAfterEffect(Hero hero, GameData gameData)
        {
            hero.Heal(value);
        }

        private void AfterReceiveDmgDelegate(Hero hero, Hero? attacker, int dmg, Consts.DamageType dmgType, GameData gameData)
        {
            if (hero.HP <= 0)
                hero.HP = 1;
        }
    }
}
