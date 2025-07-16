using BattleArenaServer.Models;

namespace BattleArenaServer.Interfaces
{
    public interface IPick
    {
        public void FillHeroesList();
        public List<Hero> GetHeroes();
        public bool PickHero(string heroName);
        public bool BanHero(string heroName);
        public void StartBattle();
        public void ChangePickBanStage();
    }
}
