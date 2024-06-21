using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.Skills.GeomantSkills.Auras;

namespace BattleArenaServer.Skills.GeomantSkills
{
    public class StoneStrenghtPSkill : PassiveSkill
    {
        string upgString = "";
        Aura Aura = new StoneStrengthAura(false);
        public StoneStrenghtPSkill(Hero hero) : base(hero)
        {
            name = "Stone Strenght";
            title = "Герой наполняется силой природы. Каждый столоктит в радиусе 3-х гексов даёт +1 к броне, +1 к сопротивлению, +10 к урону." + upgString;
            titleUpg = "Соседние столоктиты дают двойной бонус.";
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
                upgString = "\n" + titleUpg;
                Aura.CancelEffect(hero);
                hero.AuraList.Remove(Aura);
                Aura = new StoneStrengthAura(true);
                hero.AuraList.Add(Aura);
                AttackService.ContinuousAuraAction();
                title = "Герой наполняется силой природы. Каждый столоктит в радиусе 3-х гексов даёт +1 к броне, +1 к сопротивлению, +10 к урону." + upgString;
                return true;
            }
            return false;
        }
    }
}
