using BattleArenaServer.Effects.Unique;
using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.ChaosSkills
{
    public class ChaosPowerPSkill : PassiveSkill
    {
        int chaosPoints = 12;
        ChaosPowerUnique chaosPowerUnique;
        public ChaosPowerPSkill(Hero hero) : base(hero)
        {
            name = "Chaos Power";
            title = $"В начале каждого хода Вы распределяете {chaosPoints} очков хаоса случайным образом между тремя характеристиками." +
                $"Урон, броня и сопротивление. Каждое очко хаоса даёт +1 для брони и сопротивления и +8 для урона.";
            titleUpg = "Увеличивает количество очков хаоса на 5.";
        }

        public override bool Cast(RequestData requestData)
        {
            return false;
        }

        public override void refreshEffect()
        {
            if (hero.EffectList.Contains(chaosPowerUnique))
            {
                chaosPowerUnique.RemoveEffect(hero);
                hero.EffectList.Remove(chaosPowerUnique);
            }
            chaosPowerUnique = new ChaosPowerUnique(hero.Id, chaosPoints, 99);
            hero.AddEffect(chaosPowerUnique);
        }

        public override bool UpgradeSkill()
        {
            upgraded = true;
            chaosPoints += 5;
            title = $"В начале каждого хода Вы распределяете {chaosPoints} очков хаоса случайным образом между тремя характеристиками." +
                $"Урон, броня и сопротивление. Каждое очко хаоса даёт +1 для брони и сопротивления и +8 для урона.";
            return true;
        }
    }
}
