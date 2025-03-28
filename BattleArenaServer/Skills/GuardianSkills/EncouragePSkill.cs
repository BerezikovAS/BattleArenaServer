using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.Skills.GuardianSkills.Auras;

namespace BattleArenaServer.Skills.GuardianSkills
{
    public class EncouragePSkill : PassiveSkill
    {
        int extraArmor = 1;
        int extraDmg = 5;
        Aura Aura;

        public EncouragePSkill(Hero hero) : base(hero)
        {
            name = "Encourage";
            title = $"Вы и Ваши союзники по соседству получаете +{extraArmor} брони и +{extraDmg} к урону за каждого врага вокруг.";
            titleUpg = "+5 к урону за врага.";
            Aura = new EncourageAura(extraArmor, extraDmg);
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
                extraDmg += 5;
                Aura.CancelEffect(hero);
                hero.AuraList.Remove(Aura);
                Aura = new EncourageAura(extraArmor, extraDmg);
                hero.AuraList.Add(Aura);
                AttackService.ContinuousAuraAction();
                title = $"Вы и Ваши союзники по соседству получаете +{extraArmor} брони и +{extraDmg} к урону за каждого врага вокруг.";
                return true;
            }
            return false;
        }
    }
}
