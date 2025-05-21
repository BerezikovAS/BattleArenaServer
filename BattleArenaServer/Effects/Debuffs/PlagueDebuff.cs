using BattleArenaServer.Models;
using BattleArenaServer.Skills.PlagueDoctorSkills.Auras;

namespace BattleArenaServer.Effects.Debuffs
{
    public class PlagueDebuff : Effect
    {
        PlagueAura plagueAura;
        public PlagueDebuff(int _idCaster, int _value, int _duration, int _turn)
        {
            Name = "Plague";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = $"Сопротивление уменьшено на {value}. В конце хода Вы заразите своих союзников.";
            plagueAura = new PlagueAura(value, _turn);
        }

        public override void ApplyEffect(Hero _hero)
        {
            _hero.Resist -= value;
            _hero.AuraList.Add(plagueAura);
        }

        public override void RemoveEffect(Hero _hero)
        {
            _hero.Resist += value;
            _hero.AuraList.Remove(plagueAura);

        }
    }
}
