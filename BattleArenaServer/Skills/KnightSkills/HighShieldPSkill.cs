using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.Skills.KnightSkills.Auras;

namespace BattleArenaServer.Skills.KnightSkills
{
    public class HighShieldPSkill : PassiveSkill
    {
        int armor = 3;
        Aura Aura = new HighShieldAura(false);
        public HighShieldPSkill(Hero hero) : base(hero)
        {
            name = "High Shield";
            title = $"Герой защищает себя и союзников рядом от дальнобойных атак. При обороне имеет +{armor} брони, если атакующий находится не ближе двух клеток от цели атаки.";
            titleUpg = "+3 к бонусу брони.";
            hero.AuraList.Add(Aura);
        }

        public override bool Cast(RequestData requestData)
        {
            return false;
        }

        public override bool UpgradeSkill()
        {
            if (!upgraded)
            {
                upgraded = true;
                armor += 3;
                Aura.CancelEffect(hero);
                hero.AuraList.Remove(Aura);
                Aura = new HighShieldAura(true);
                hero.AuraList.Add(Aura);
                AttackService.ContinuousAuraAction();
                title = $"Герой защищает себя и союзников рядом от дальнобойных атак. При обороне имеет +{armor} брони, если атакующий находится не ближе двух клеток от цели атаки.";
                return true;
            }
            return false;
        }
    }
}
