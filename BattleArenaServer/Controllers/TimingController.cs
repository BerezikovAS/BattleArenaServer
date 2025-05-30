﻿using BattleArenaServer.Interfaces;
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

        // GET: TimingController/HealthCheck
        [HttpGet]
        public ActionResult<string> HealthCheck()
        {
            return "Everything is good!";
        }

        // GET: TimingController/EndTurn
        [HttpGet]
        public int EndTurn()
        {
            return _timingService.EndTurn();
        }

        // GET: TimingController/GetActiveHero
        [HttpGet]
        public int GetActiveHero()
        {
            return _timingService.GetActiveHero();
        }
    }
}
