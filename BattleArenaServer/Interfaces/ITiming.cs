using BattleArenaServer.Models;

namespace BattleArenaServer.Interfaces
{
    public interface ITiming
    {
        public int EndTurn();

        public int GetActiveHero();
    }
}
