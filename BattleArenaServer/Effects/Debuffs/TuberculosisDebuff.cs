using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Effects.Debuffs
{
    public class TuberculosisDebuff : Effect
    {
        Hero caster;
        public TuberculosisDebuff(int _idCaster, int _value, int _duration, Hero _caster)
        {
            Name = "Tuberculosis";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            caster = _caster;
            description = $"Каждое применение способности наносит Вам {value} чистого урона и отнимает 1 ОД.";
        }

        public override void ApplyEffect(Hero hero, GameData gameData)
        {
            hero.afterSpellCast += AfterSpellCastDelegate;
        }

        public override void RemoveEffect(Hero hero, GameData gameData)
        {
            hero.afterSpellCast -= AfterSpellCastDelegate;
        }

        private void AfterSpellCastDelegate(Hero attacker, Hero? defender, Skill skill, GameData gameData)
        {
            attacker.SpendAP(1, gameData);
            AttackService.SetDamage(caster, attacker, value, Consts.DamageType.Pure, gameData);
        }
    }
}
