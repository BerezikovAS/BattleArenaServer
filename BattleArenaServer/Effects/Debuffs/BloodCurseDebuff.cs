using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Debuffs
{
    public class BloodCurseDebuff : Effect
    {
        public BloodCurseDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "BloodCurse";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = $"{value}% от процента потерянного ХП восстанавливается заклинателю.";
        }

        public override void ApplyEffect(Hero _hero)
        {
            _hero.applyDamage += ApplyDamageDelegate;
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.applyDamage -= ApplyDamageDelegate;
        }

        private bool ApplyDamageDelegate(Hero? attacker, Hero defender, int dmg)
        {
            Hero? caster = GameData._heroes.FirstOrDefault(x => x.Id == idCaster && x.HP > 0);
            if (defender != null && caster != null)
            {
                double percentRestore = ((double)dmg / defender.MaxHP) * ((double)value / 100);
                int hpRestore = Convert.ToInt32((double)caster.MaxHP * percentRestore);
                caster.Heal(hpRestore);
            }
            return false;
        }
    }
}
