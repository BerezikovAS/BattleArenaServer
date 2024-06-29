using BattleArenaServer.Interfaces;
using BattleArenaServer.SkillCastRequests;
using static BattleArenaServer.Models.Consts;

namespace BattleArenaServer.Models
{
    public abstract class Skill
    {
        public string name { get; set; } = "-None-";
        public string title { get; set; } = "Заглушка";
        public string titleUpg { get; set; } = "Заглушка";
        public int coolDown { get; set; }
        public int coolDownNow { get; set; } = 0;
        public int requireAP { get; set; }
        public bool nonTarget { get; set; } = false;
        public int range { get; set; } = 0;
        public int radius { get; set; } = 0;
        public bool upgraded { get; set; } = false;
        public SpellArea area {  get; set; } = 0;
        public SkillStats stats { get; set; } = new SkillStats(0, 0, 0, 0);
        public Consts.SkillType skillType { get; set; } = SkillType.Active;

        public int dmg { get; set; } = 0;
        public string extraDmgStr { get; set; } = "";
        public Consts.DamageType dmgType { get; set; } = DamageType.Physical;

        public abstract bool Cast(RequestData requestData);

        public abstract bool UpgradeSkill();

        public ISkillCastRequest request { get; set; } = new NontargetCastRequest();

        public void SetCoolDown(int _coolDownNow) { coolDownNow = _coolDownNow; }

    }
}
