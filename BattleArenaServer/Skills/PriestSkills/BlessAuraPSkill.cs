using BattleArenaServer.Models;
using BattleArenaServer.Skills.PriestSkills.Auras;

namespace BattleArenaServer.Skills.PriestSkills
{
    public class BlessAuraPSkill : PassiveSkill
    {
        double basicHeal = 0.05;
        double extraHeal = 0.01;
        Aura Aura;
        public BlessAuraPSkill(Hero hero) : base(hero)
        {
            name = "Bless Aura";
            title = $"Святое благословение излечивает героя и союзников рядом на {basicHeal * 100}% потерянного здоровья.\n" +
                $"Лечение усиливается на {extraHeal * 100}% за каждый негативный эффект у цели.";
            titleUpg = "+2% к лечению. +1% к лечению за негативный эффект.";
            Aura = new BlessAura(basicHeal, extraHeal);
            hero.AuraList.Add(Aura);
        }

        public override bool Cast(RequestData requestData)
        {
            return false;
        }

        public override void refreshEffect()
        {
        }

        public override bool UpgradeSkill()
        {
            upgraded = true;
            basicHeal += 0.02;
            extraHeal += 0.01;
            hero.AuraList.Remove(Aura);
            Aura = new BlessAura(basicHeal, extraHeal);
            hero.AuraList.Add(Aura);
            title = $"Святое благословение излечивает героя и союзников рядом на {basicHeal * 100}% потерянного здоровья.\n" +
                $"Лечение усиливается на {extraHeal * 100}% за каждый негативный эффект у цели.";
            return true;
        }
    }
}
