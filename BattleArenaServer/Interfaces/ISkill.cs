using BattleArenaServer.Models;

namespace BattleArenaServer.Interfaces
{
    public interface ISkill
    {
        public bool Cast(List<Hex> _hexes, int _target, int _caster);

        public void Cancel();

        public ISkillCastRequest request { get; }

        public int GetCoolDownNow { get; }

        public int GetCoolDown { get; }

        public int GetRequireAP { get; }

        public void SetCoolDown(int _coolDownNow);
    }
}
