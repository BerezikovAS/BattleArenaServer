using BattleArenaServer.Effects;
using BattleArenaServer.Effects.Unique;
using BattleArenaServer.Models;
using BattleArenaServer.Models.Obstacles;
using BattleArenaServer.Skills.PriestSkills.Auras;
using System.Xml.Linq;

namespace BattleArenaServer.Skills.ChaosSkills
{
    public class ChaosPowerPSkill : PassiveSkill
    {
        ChaosPowerUnique chaosPowerUnique;
        public ChaosPowerPSkill(Hero hero) : base(hero)
        {
            name = "Chaos Power";
            title = $"В начале каждого хода Вы распределяете свои очки хаоса случайным образом между тремя характеристиками." +
                $"Урон, броня и сопротивление. Каждое очко хаоса даёт +1 для брони и сопротивления и +8 для урона.";
            titleUpg = "Увеличивает количество очков хаоса на 3, за каждого поверженного героя.";

            chaosPowerUnique = new ChaosPowerUnique(hero.Id, 15, -1);
            chaosPowerUnique.ApplyEffect(hero);
        }

        public override bool Cast(RequestData requestData)
        {
            return false;
        }

        public override void refreshEffect()
        {
            int chaosPoints = 15;
            foreach (Hero h in GameData._heroes)
            {
                if (h is not SolidObstacle && h.HP <= 0)
                    chaosPoints += 3;
            }

            chaosPowerUnique.RemoveEffect(hero);
            chaosPowerUnique = new ChaosPowerUnique(hero.Id, chaosPoints, -1);
            chaosPowerUnique.ApplyEffect(hero);
        }

        public override bool UpgradeSkill()
        {
            upgraded = true;
            title = $"В начале каждого хода Вы распределяете свои очки хаоса случайным образом между тремя характеристиками." +
                $"Урон, броня и сопротивление. Каждое очко хаоса даёт +1 для брони и сопротивления и +8 для урона." +
                $"Вы получаете по 3 доп. очка хаоса за каждого поверженного героя.";
            return true;
        }
    }
}
