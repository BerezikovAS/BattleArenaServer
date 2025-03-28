using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Skills.ShadowSkills
{
    public class ShadowsEverywherePSkill : PassiveSkill
    {
        int extraDmg = 5;
        public ShadowsEverywherePSkill(Hero hero) : base(hero)
        {
            name = "Shadows Everywhere";
            title = $"Тени окружают Ваших жертв. Нанесите {extraDmg} бонусного урона от атак за каждую свободную клетку вокруг цели.";
            titleUpg = "+3 к бонусному урону за свободную клетку";
            skillType = Consts.SkillType.Passive;
            hero.passiveAttackDamage += ShadowsEverywhere;
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
                hero.passiveAttackDamage -= ShadowsEverywhere;
                extraDmg += 3;
                hero.passiveAttackDamage += ShadowsEverywhere;
                title = $"Тени окружают Ваших жертв. Нанесите {extraDmg} бонусного урона от атак за каждую свободную клетку вокруг цели.";
                return true;
            }
            return false;
        }

        private int ShadowsEverywhere(Hero attacker, Hero? defender)
        {
            Hex? defenderHex = GameData._hexes.FirstOrDefault(x => x.HERO?.Id == defender.Id);

            int freeHexes = 0;
            if (defenderHex != null)
            {
                foreach (var n in UtilityService.GetHexesRadius(defenderHex, 1))
                {
                    if (n.IsFree())
                        freeHexes++;
                }
                return freeHexes * extraDmg;
            }
            return 0;
        }
    }
}
