using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.Priest
{
    public class Smight : Skill
    {
        public Smight()
        {
            name = "Smight";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 2;
        }

        public ISkillCastRequest request => new EnemyTargetCastRequest();

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override bool Cast(List<Hex> _hexes, int _target, int _caster)
        {
            if (request.startRequest(_hexes, _target, _caster, this))
            {
                Hero caster = _hexes[_caster].HERO;
                Hero target = _hexes[_target].HERO;
                if (caster != null & target != null)
                {
                    int dmg = 150;
                    foreach (var skill in target.SkillList)
                    {
                        if (skill.coolDownNow > 0)
                            dmg += 25;
                    }

                    caster.AP -= requireAP;
                    _hexes[_target].SetDamage(dmg, "pure");
                    coolDownNow = coolDown;
                    return true;
                }
            }
            return false;
        }
    }
}
