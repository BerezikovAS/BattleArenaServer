using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Effects.Debuffs
{
    public class RuptureDebuff : Effect
    {
        public RuptureDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "Rupture";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = $"Перемещение отнимает {value} ХП, за каждый пройденный гекс.";
        }

        public override void ApplyEffect(Hero hero)
        {
            hero.afterMove += Rupture;
        }

        public override void RemoveEffect(Hero hero)
        {
            hero.afterMove -= Rupture;
        }

        private void Rupture(Hero hero, Hex? currentHex, Hex targetHex)
        {
            Hero? attacker = GameData._heroes.FirstOrDefault(x => x.Id == idCaster);
            int distance = currentHex.Distance(targetHex);
            AttackService.SetDamage(attacker, hero, distance * value, Consts.DamageType.Pure);
        }
    }
}
