using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Buffs
{
    public class DodgeBuff : Effect
    {
        public DodgeBuff(int _idCaster, int _value, int _duration)
        {
            Name = "Dodge";
            type = Consts.StatusEffect.Buff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = $"Уклоняется от первой атаки.";

            effectTags.Add(Consts.EffectTag.Dodge);
        }

        public override void ApplyEffect(Hero _hero, GameData gameData)
        {

        }

        public override void RemoveEffect(Hero _hero, GameData gameData)
        {

        }
    }
}
