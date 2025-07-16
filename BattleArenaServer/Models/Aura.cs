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

        public abstract void ApplyEffect(Hero source, Hero target, GameData gameData);
        public abstract void CancelEffect(Hero source, GameData gameData);

        public virtual void SetEffect(Hero heroSource, Hex hexSource, GameData gameData)
        {
            foreach (var n in UtilityService.GetHexesRadius(hexSource, radius, gameData))
            {
                if (n.HERO != null)
                    ApplyEffect(heroSource, n.HERO, gameData);
            }
        }
    }
}
