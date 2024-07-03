using BattleArenaServer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BattleArenaServer.Controllers
{
    [Route("[controller]/[action]")]
    public class TimingController : Controller
    {
        private readonly ITiming _timingService;

        public TimingController(ITiming timingService)
        {
            _timingService = timingService;
        }

        // GET: TimingController/EndTurn
        [HttpGet]
        public void EndTurn()
        {
            _timingService.EndTurn();
        }

        // GET: TimingController/GetActiveHero
        [HttpGet]
        public int GetActiveHero()
        {
            return _timingService.GetActiveHero();
        }
    }
}
