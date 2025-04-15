using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.Skills.NecromancerSkills.Auras;

namespace BattleArenaServer.Skills.NecromancerSkills
{
    public class DeadlyAuraPSkill : PassiveSkill
    {
        int reduce_resist = 2;
        Aura Aura;
        public DeadlyAuraPSkill(Hero hero) : base(hero)
        {
            name = "Deadly Aura";
            title = $"Враги вокруг Вас имеют -{reduce_resist} к сопротивлению.";
            titleUpg = "+1 к снижению сопротивления, +1 к радиусу ауры.";
            radius = 1;
            Aura = new DeadlyAura(false, reduce_resist, radius);
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
                reduce_resist += 1;
                Aura.CancelEffect(hero);
                hero.AuraList.Remove(Aura);
                Aura = new DeadlyAura(false, reduce_resist, radius);
                hero.AuraList.Add(Aura);
                AttackService.ContinuousAuraAction();
                title = $"Враги в радиусе 2-х клеток от Вас имеют -{reduce_resist} к сопротивлению.";
                return true;
            }
            return false;
        }
    }
}
