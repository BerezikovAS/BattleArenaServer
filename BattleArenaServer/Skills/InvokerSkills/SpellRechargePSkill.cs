using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.InvokerSkills
{
    public class SpellRechargePSkill : PassiveSkill
    {
        int cooldownReduce = 1;
        public SpellRechargePSkill(Hero hero) : base(hero)
        {
            name = "Spell Recharge";
            title = $"Каждая атака героя снижает время перезарядки спосособностей на {cooldownReduce}.";
            titleUpg = "Снижение времени отката за атаку = 2.";
            hero.afterAttack += AfterAttackDelegate;
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
                cooldownReduce += 1;
                hero.afterAttack -= AfterAttackDelegate;
                hero.afterAttack += AfterAttackDelegate;
                title = $"Каждая атака героя снижает время перезарядки спосособностей на {cooldownReduce}.";
                return true;
            }
            return false;
        }

        private bool AfterAttackDelegate(Hero attacker, Hero? defender, int dmg)
        {
            foreach (var spell in attacker.SkillList)
            {
                if (spell.coolDownNow > 0)
                    spell.coolDownNow -= cooldownReduce;
                if (spell.coolDownNow < 0)
                    spell.coolDownNow = 0;
            }

            return true;
        }
    }
}
