using BattleArenaServer.Interfaces;

namespace BattleArenaServer.Models
{
    public abstract class Skill
    {
        public string name { get; set; }
        public int coolDown { get; set; }
        public int coolDownNow { get; set; } = 0;
        public int requireAP { get; set; }
        public bool nonTarget { get; set; } = false;
        public int range { get; set; } = 0;
        public int radius { get; set; } = 0;
        public bool upgraded { get; set; } = false;

        public abstract bool Cast(List<Hex> _hexes, int _target, int _caster);

        public abstract void Cancel();

        public ISkillCastRequest request { get; }

        public void SetCoolDown(int _coolDownNow) { coolDownNow = _coolDownNow; }
    }
}
