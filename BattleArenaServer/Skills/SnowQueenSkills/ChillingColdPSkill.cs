using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.Skills.SnowQueenSkills.Auras;

namespace BattleArenaServer.Skills.SnowQueenSkills
{
    public class ChillingColdPSkill : PassiveSkill
    {
        int dmg_reduce = 15;
        Aura Aura;
        public ChillingColdPSkill(Hero hero) : base(hero)
        {
            name = "Chilling Cold";
            title = $"Морозная аура снижает урон от атак врагов в радиусе 2-х клеток на {dmg_reduce}%";
            titleUpg = "+10% к снижению урона от атак";
            radius = 2;
            Aura = new ChillingColdAura(false, dmg_reduce, radius);
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
            if (!upgraded)
            {
                upgraded = true;
                Aura.CancelEffect(hero);
                hero.AuraList.Remove(Aura);
                dmg_reduce += 10;
                Aura = new ChillingColdAura(false, dmg_reduce, radius);
                hero.AuraList.Add(Aura);
                AttackService.ContinuousAuraAction();
                title = $"Морозная аура снижает урон от атак врагов в радиусе 2-х клеток на {dmg_reduce}%";
                return true;
            }
            return false;
        }
    }
}
