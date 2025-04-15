using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Effects.Debuffs
{
    public class SteamDebuff : Effect
    {
        double dmgReceived = 0;
        public SteamDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "Steam";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = $"По окончанию действия эффекта Вы получите {value}% от потерянного ХП в качестве магического урона. (Потеряно ХП: {dmgReceived})";
        }

        public override void ApplyEffect(Hero hero)
        {
            hero.afterReceiveDmg += AfterReceiveDmgDelegate;
        }

        public override void RemoveEffect(Hero hero)
        {
            hero.afterReceiveDmg -= AfterReceiveDmgDelegate;
        }

        public override void RefreshDescr()
        {
            description = $"По окончанию действия эффекта Вы получите {value}% от потерянного ХП в качестве магического урона. (Потеряно ХП: {dmgReceived})";
        }

        public override void ApplyAfterEffect(Hero hero)
        {
            int dmg = (int)(dmgReceived * Convert.ToDouble(value) / 100);
            Hero? attacker = GameData._heroes.FirstOrDefault(x => x.Id == idCaster);
            AttackService.SetDamage(attacker, hero, dmg, Consts.DamageType.Magic);
        }

        private void AfterReceiveDmgDelegate(Hero defender, Hero? attacker, int dmg, Consts.DamageType dmgType)
        {
            dmgReceived += dmg;
        }
    }
}
