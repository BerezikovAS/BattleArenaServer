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
                extraDmg += 5;
                Aura.CancelEffect(hero, gameData);
                hero.AuraList.Remove(Aura);
                Aura = new EncourageAura(extraArmor, extraDmg);
                hero.AuraList.Add(Aura);
                AttackService.ContinuousAuraAction(gameData);
                title = $"Вы и Ваши союзники по соседству получаете +{extraArmor} брони и +{extraDmg} к урону за каждого врага вокруг.";
                return true;
            }
            return false;
        }
    }
}
