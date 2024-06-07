using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

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
        public ActionResult<List<Hex>> StepHero(int _cur_pos, int _targer_pos)
        {
            return _fieldService.StepHero(_cur_pos, _targer_pos);
        }

        // GET: FieldController/GetField
        [HttpGet]
        public ActionResult<List<Hex>> AttackHero(int _cur_pos, int _targer_pos)
        {
            return _fieldService.AttackHero(_cur_pos, _targer_pos);
        }

        // GET: FieldController/GetField
        [HttpGet]
        public bool SpellCast(int _target, int _caster, int _spell)
        {
            return _fieldService.SpellCast(_target, _caster, _spell);
        }

        // GET: FieldController/GetField
        [HttpGet]
        public ActionResult<List<Hex>> SpellArea(int _target, int _caster, int _spell)
        {
            return _fieldService.SpellArea(_target, _caster, _spell);
        }

        // GET: FieldController/GetField
        [HttpGet]
        public ActionResult<bool> UpgradeSkill(int _caster, int _skill)
        {
            return _fieldService.UpgradeSkill(_caster, _skill);
        }
    }
}
