using BattleArenaServer.Models;

namespace BattleArenaServer.Interfaces
{
    public interface ITiming
    {
        public void EndTurn();

        public int GetActiveHero();
    }
}
