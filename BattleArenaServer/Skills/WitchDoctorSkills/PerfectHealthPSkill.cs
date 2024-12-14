using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.WitchDoctorSkills
{
    public class PerfectHealthPSkill : PassiveSkill
    {
        int basicBunusHP = 100;
        int upgradeBonusHP = 75;
        public PerfectHealthPSkill(Hero hero) : base(hero)
        {
            name = "Perfect Health";
            title = $"Вы обеспечиваете отменное здоровье своему отряду. Все союзники имеют +{basicBunusHP} ХП.";
            titleUpg = $"Увеличиает ХП союзников еще на {upgradeBonusHP}";
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
            upgraded = true;
            title = $"Вы обеспечиваете отменное здоровье своему отряду. Все союзники имеют +{basicBunusHP + upgradeBonusHP} ХП.";
            ApplyModifier();
            return true;
        }

        public void ApplyModifier()
        {
            if (upgraded)
            {
                foreach (var ally in GameData._heroes.Where(x => x.Team == hero.Team))
                {
                    ally.MaxHP += upgradeBonusHP;
                    ally.HP += upgradeBonusHP;
                }
            }
            else
            {
                foreach (var ally in GameData._heroes.Where(x => x.Team == hero.Team))
                {
                    ally.MaxHP += basicBunusHP;
                    ally.HP += basicBunusHP;
                }
            }
        }
    }
}
