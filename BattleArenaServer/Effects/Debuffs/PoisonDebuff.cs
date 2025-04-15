using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Effects.Debuffs
{
    public class PoisonDebuff : Effect
    {
        public PoisonDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "Poison";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            effectType = Consts.EffectType.EndTurn;
            description = $"Теряет {value}% ХП в конце своего хода.";
        }

        public override void ApplyEffect(Hero defender)
        {
            int dmg = (int)(Convert.ToDouble(defender.HP) * value / 100);
            Hero? attacker = GameData._heroes.FirstOrDefault(x => x.Id == idCaster);
            AttackService.SetDamage(attacker, defender, dmg, Consts.DamageType.Pure);
        }

        public override void RemoveEffect(Hero _hero)
        {

        }
    }
}
