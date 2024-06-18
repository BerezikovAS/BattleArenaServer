using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.PriestSkills.Auras
{
    public class BlessAura : Aura
    {
        double basicHeal;
        double extraHeal;
        public BlessAura(double basicHeal, double extraHeal)
        {
            Name = "BlessAura";
            radius = 1;
            type = Consts.AuraType.EndTurn;
            this.basicHeal = basicHeal;
            this.extraHeal = extraHeal;
        }

        public override void ApplyEffect(Hero source, Hero target)
        {
            if (source.Team == target.Team)
            {
                double healPercent = basicHeal;
                foreach (var effect in target.EffectList)
                {
                    if (effect.type == "debuff")
                        healPercent += extraHeal;
                }
                double restoreHP = Math.Round((target.MaxHP - target.HP) * healPercent);
                target.Heal((int)restoreHP);
            }    
        }

        public override void CancelEffect(Hero source)
        {

        }
    }
}
