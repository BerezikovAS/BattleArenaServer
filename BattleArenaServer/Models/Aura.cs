using BattleArenaServer.Services;

namespace BattleArenaServer.Models
{
    public abstract class Aura
    {
        public string Name { get; set; } = "";
        public string title { get; set; } = "Заглушка";
        public string titleUpg { get; set; } = "Заглушка";
        public int radius { get; set; } = 0;
        public Consts.AuraType type { get; set; } = Consts.AuraType.Continuous;

        public abstract void ApplyEffect(Hero source, Hero target);
        public abstract void CancelEffect();

        public void SetEffect(Hero heroSource, Hex hexSource)
        {
            foreach (var n in UtilityService.GetHexesRadius(hexSource, radius))
            {
                if (n.HERO != null)
                    ApplyEffect(heroSource, n.HERO);
            }
        }
    }
}
