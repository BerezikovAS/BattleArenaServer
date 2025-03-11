using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Effects.Buffs
{
    public class ChainOfPainBuff : Effect
    {
        int target = -1;
        public ChainOfPainBuff(int _idCaster, int _value, int _duration, int _target)
        {
            Name = "ChainOfPain";
            type = Consts.StatusEffect.Buff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            target = _target;
            description = $"{value}% полученного урона Вам наносится и связанному врагу.";
        }

        public override void ApplyEffect(Hero _hero)
        {
            _hero.afterReceiveDmg += AfterReceiveDmgDelegate;
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.afterReceiveDmg -= AfterReceiveDmgDelegate;
        }

        private void AfterReceiveDmgDelegate(Hero defender, Hero? attacker, int dmg)
        {
            Hero? targetHero = GameData._heroes.FirstOrDefault(x => x.Id == target);
            if (targetHero != null)
            {
                int dmgTransfer = Convert.ToInt32(Convert.ToDouble(dmg * value) / 100);
                AttackService.SetDamage(defender, targetHero, dmgTransfer, Consts.DamageType.Pure);
            }
        }
    }
}
