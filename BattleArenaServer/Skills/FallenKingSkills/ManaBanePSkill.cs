using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.Skills.FallenKingSkills.Auras;

namespace BattleArenaServer.Skills.FallenKingSkills
{
    public class ManaBanePSkill : PassiveSkill
    {
        Aura Aura;
        public ManaBanePSkill(Hero hero) : base(hero)
        {
            name = "Mana Bane";
            dmg = 30;
            radius = 2;
            title = $"Враги в радиусе {radius} клеток получают {dmg} магического урона, когда используют заклинание.";
            titleUpg = "+1 к радиусу, +10 к урону";
            Aura = new ManaBaneAura(dmg, radius, hero);
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
                radius += 1;
                dmg += 10;
                Aura.CancelEffect(hero);
                hero.AuraList.Remove(Aura);
                Aura = new ManaBaneAura(dmg, radius, hero);
                hero.AuraList.Add(Aura);
                AttackService.ContinuousAuraAction();
                title = $"Враги в радиусе {radius} клеток получают {dmg} магического урона, когда используют заклинание.";
                return true;
            }
            return false;
        }
    }
}
