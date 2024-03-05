using BattleArenaServer.Effects;
using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.Knight
{
    public class SelfHeal : Skill
    {
        public SelfHeal() 
        {
            name = "Restoration";
            coolDown = 4;
            coolDownNow = 0; 
            requireAP = 1;
            nonTarget = true;
        }

        public ISkillCastRequest request => new NontargetCastRequest();     
        
        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override bool Cast(List<Hex> _hexes, int _target, int _caster)
        {
            if(request.startRequest(_hexes, _target, _caster, this))
            {
                Hero hero = _hexes[_caster].HERO;
                if(hero != null)
                {
                    hero.AP -= requireAP;
                    hero.Heal(200);
                    coolDownNow = coolDown;

                    ArmorBuff buffArmor = new ArmorBuff(hero.Id, 3, 2);
                    hero.EffectList.Add(buffArmor);
                    buffArmor.ApplyEffect(hero);

                    return true;
                }
            }
            return false;
        }
    }
}
