using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.CultistSkills
{
    public class ScornPSkill : PassiveSkill
    {
        int cntEnemiesPenalty = 1;
        string skillDesc = ", кроме первого";
        public ScornPSkill(Hero hero) : base(hero)
        {
            name = "Scorn";
            dmg = 10;
            title = $"Ваши атаки наносят дополнительно {dmg} урона за каждый отрицательный эффект на цели.";
            titleUpg = "+8 к бонусному урону.";
            skillType = Consts.SkillType.Passive;
            hero.passiveAttackDamage += Scorn;
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
                dmg += 8;
                hero.passiveAttackDamage -= Scorn;
                hero.passiveAttackDamage += Scorn;
                title = $"Ваши атаки наносят дополнительно {dmg} урона за каждый отрицательный эффект на цели.";
                return true;
            }
            return false;
        }

        private int Scorn(Hero attacker, Hero? defender)
        {
            int extraDmg = 0;
            foreach (var effect in defender.EffectList)
            {
                if (effect.type == Consts.StatusEffect.Debuff)
                    extraDmg += dmg;
            }
            
            return extraDmg;
        }
    }
}
