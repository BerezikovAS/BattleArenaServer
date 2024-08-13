using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.AbominationSkills
{
    public class FleshEaterPSkill : PassiveSkill
    {
        int maxHPreduction = 50;
        public FleshEaterPSkill(Hero hero) : base(hero)
        {
            name = "Flesh Eater";
            title = $"Каждая атака по врагу крадет у того {maxHPreduction} от максимального запаса ХП.";
            titleUpg = "+20 к краже максимального запаса ХП";
            hero.afterAttack += FleshEater;
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
                hero.afterAttack -= FleshEater;
                maxHPreduction += 20;
                hero.afterAttack += FleshEater;
                title = $"Каждая атака по врагу крадет у того {maxHPreduction} от максимального запаса ХП.";
                return true;
            }
            return false;
        }

        private bool FleshEater(Hero attacker, Hero defender, int dmg)
        {
            if (defender == null)
                return false;

            int stealMaxHP = maxHPreduction;
            if (defender.MaxHP <= maxHPreduction)
                stealMaxHP = defender.MaxHP - 1;

            defender.MaxHP -= stealMaxHP;
            attacker.MaxHP += stealMaxHP;

            if (defender.HP > defender.MaxHP)
                defender.HP = defender.MaxHP;

            return true;
        }
    }
}
