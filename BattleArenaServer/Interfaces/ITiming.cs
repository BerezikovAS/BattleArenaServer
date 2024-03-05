using BattleArenaServer.Models;

namespace BattleArenaServer.Interfaces
{
    public interface ITiming
    {
        public int EndTurn();

        public void SetHeroList(List<Hero> _heroes);
    }
}
