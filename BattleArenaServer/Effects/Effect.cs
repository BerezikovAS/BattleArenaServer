using BattleArenaServer.Models;

namespace BattleArenaServer.Effects
{
    public abstract class Effect
    {
        public string Name { get; set; }
        public string type { get; set; }
        public int idCaster { get; set; }

        public int value { get; set; }
        public int duration { get; set; }

        public string description { get; set; } = "";

        public Consts.EffectType effectType { get; set; } = Consts.EffectType.Instant;

        public abstract void ApplyEffect(Hero hero);

        public abstract void RemoveEffect(Hero hero);
    }
}
