using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Buffs
{
    public class DmgShieldBuff : Effect
    {
        public DmgShieldBuff(int _idCaster, int _value, int _duration)
        {
            Name = "DmgShield";
            type = Consts.StatusEffect.Buff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = $"Щит поглощает {value} любого урона";

            effectTags.Add(Consts.EffectTag.DmgShield);
        }

        public override void ApplyEffect(Hero _hero)
        {

        }

        public override void RemoveEffect(Hero _hero)
        {

        }

        public override void RefreshDescr()
        {
            description = $"Щит поглощает {value} любого урона";
        }
    }
}
