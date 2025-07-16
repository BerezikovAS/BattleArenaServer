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

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            return false;
        }

        public override void refreshEffect(GameData gameData)
        {
        }

        public override bool UpgradeSkill(GameData gameData)
        {
            if (!upgraded)
            {
                upgraded = true;
                Aura.CancelEffect(hero, gameData);
                hero.AuraList.Remove(Aura);
                dmg_reduce += 10;
                Aura = new ChillingColdAura(false, dmg_reduce, radius);
                hero.AuraList.Add(Aura);
                AttackService.ContinuousAuraAction(gameData);
                title = $"Морозная аура снижает урон от атак врагов в радиусе 2-х клеток на {dmg_reduce}%";
                return true;
            }
            return false;
        }
    }
}
