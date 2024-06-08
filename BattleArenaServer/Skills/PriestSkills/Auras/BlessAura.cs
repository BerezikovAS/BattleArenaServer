using BattleArenaServer.Models;
using System.Xml.Linq;

namespace BattleArenaServer.Skills.PriestSkills.Auras
{
    public class BlessAura : Aura
    {
        public BlessAura()
        {
            Name = "BlessAura";
            radius = 1;
            type = Consts.AuraType.EndTurn;
        }

        public override void ApplyEffect(Hero source, Hero target)
        {
            if (source.Team == target.Team)
            {
                double healPercent = 0.05;
                foreach (var effect in target.EffectList)
                {
                    if (effect.type == "debuff")
                        healPercent += 0.02;
                }
                double restoreHP = Math.Round((target.MaxHP - target.HP) * healPercent);
                target.Heal((int)restoreHP);
            }    
        }

        public override void CancelEffect()
        {

        }
    }
}
