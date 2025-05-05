using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.ElementalistSkills
{
    public class ElementEssencePSkill : PassiveSkill
    {
        private int extraDmg = 6;
        public ElementEssencePSkill(Hero hero) : base(hero)
        {
            name = "Element Essence";
            title = $"Каждое применение заклинания увеличивает урон от атак на {extraDmg}";
            titleUpg = "Урон увеличивается на 9";
            hero.afterSpellCast += ElementEssence;
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
                extraDmg = 9;
                title = $"Каждое применение заклинания увеличивает урон от атак на {extraDmg}";
                return true;
            }
            return false;
        }

        private void ElementEssence(Hero caster, Hero? target, Skill skill)
        {
            caster.Dmg += extraDmg;
        }
    }
}
