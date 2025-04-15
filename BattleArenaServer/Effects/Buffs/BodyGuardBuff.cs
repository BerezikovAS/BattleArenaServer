using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Effects.Buffs
{
    public class BodyGuardBuff : Effect
    {
        // Тот кого забафали и ему срезаем входящий дамаг
        public BodyGuardBuff(int _idCaster, int _value, int _duration)
        {
            Name = "BodyGuard";
            type = Consts.StatusEffect.Buff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = "Броня и сопротивление увеличены на " + value + ".\n50% полученного урона переносится на владельца способности.";
        }

        public override void ApplyEffect(Hero _hero)
        {
            _hero.StatsEffect.Armor += value;
            _hero.StatsEffect.Resist += value;
            _hero.applyDamage -= AttackService.ApplyDamage;
            _hero.applyDamage += ApplyDamageDelgate;
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.StatsEffect.Armor -= value;
            _hero.StatsEffect.Resist -= value;
            _hero.applyDamage -= ApplyDamageDelgate;
            _hero.applyDamage += AttackService.ApplyDamage;
        }

        public bool ApplyDamageDelgate(Hero attacker, Hero defender, int dmg, Consts.DamageType dmgType)
        {
            Hero? buffer = GameData._heroes.FirstOrDefault(x => x.Id == idCaster);
            if (buffer != null && buffer.HP > 0)
            {
                int halfDmg = dmg / 2;
                AttackService.ApplyDamage(attacker, buffer, halfDmg, dmgType);
                return AttackService.ApplyDamage(attacker, defender, halfDmg, dmgType);
            }
            else
                return AttackService.ApplyDamage(attacker, defender, dmg, dmgType);
        }
    }
}
