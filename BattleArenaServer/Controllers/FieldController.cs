using BattleArenaServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace BattleArenaServer.Controllers
{
    [Route("[controller]/[action]")]
    public class FieldController : Controller
    {
        private readonly IField _fieldService;

        public FieldController(IField fieldService)
        {
            _fieldService = fieldService;
        }

        // GET: FieldController/GetField
        [HttpGet]
        public ActionResult<List<Hex>> GetField()
        {
            return _fieldService.GetField();
        }

        // GET: FieldController/GetField
        [HttpGet]
        public ActionResult<List<Hex>> StepHero(int cur_pos, int targer_pos)
        {
            return _fieldService.StepHero(cur_pos, targer_pos);
        }

        // GET: FieldController/GetField
        [HttpGet]
        public ActionResult<List<Hex>> AttackHero(int cur_pos, int targer_pos)
        {
            return _fieldService.AttackHero(cur_pos, targer_pos);
        }

        // GET: FieldController/GetField
        [HttpGet]
        public bool SpellCast(int target, int caster, int spell)
        {
            return _fieldService.SpellCast(target, caster, spell);
        }

        // GET: FieldController/GetField
        [HttpGet]
        public ActionResult<bool> UpgradeSkill(int _caster, int _skill)
        {
            return _fieldService.UpgradeSkill(_caster, _skill);
        }
    }
}
