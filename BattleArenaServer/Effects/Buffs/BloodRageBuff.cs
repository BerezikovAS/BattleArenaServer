using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Effects.Buffs
{
    public class BloodRageBuff : Effect
    {
        public BloodRageBuff(int _idCaster, int _value, int _duration)
        {
            Name = "BloodRage";
            type = Consts.StatusEffect.Buff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = "Атаки стоят 1 ОД, но каждая отнимает 35 ХП у атакующего.";
            if (value > 0)
                description += "\nУрон увеличен на " + value;
        }

        public override void ApplyEffect(Hero _hero)
        {
            _hero.Dmg += value;
            _hero.APtoAttack = 1;
            _hero.afterAttack += AfterAttackDelegate;
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.Dmg -= value;
            _hero.APtoAttack = 2;
            _hero.afterAttack -= AfterAttackDelegate;
        }

        private bool AfterAttackDelegate(Hero attacker, Hero defender, int dmg, Consts.DamageType dmgType)
        {
            return AttackService.ApplyDamage(attacker, attacker, 35, dmgType);
        }
    }
}
