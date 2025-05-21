using BattleArenaServer.Models;

namespace BattleArenaServer.Effects
{
    public abstract class Effect
    {
        public string Name { get; set; }
        public Consts.StatusEffect type { get; set; }
        public int idCaster { get; set; }

        public int value { get; set; }
        public int duration { get; set; }

        public string description { get; set; } = "";

        public Consts.EffectType effectType { get; set; } = Consts.EffectType.Instant;

        public Consts.EffectDurationType durationType { get; set; } = Consts.EffectDurationType.EndTurn;

        public List<Consts.EffectTag> effectTags { get; set; } = new List<Consts.EffectTag>();

        public abstract void ApplyEffect(Hero hero);

        public abstract void RemoveEffect(Hero hero);

        public virtual void RefreshDescr() { }

        public virtual void ApplyAfterEffect(Hero hero) { }
    }
}
