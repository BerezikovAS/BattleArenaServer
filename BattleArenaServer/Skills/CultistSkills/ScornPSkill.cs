using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.CultistSkills
{
    public class ScornPSkill : PassiveSkill
    {
        public ScornPSkill(Hero hero) : base(hero)
        {
            name = "Scorn";
            dmg = 10;
            title = $"Ваши атаки наносят дополнительно {dmg} урона за каждый отрицательный эффект на цели.";
            titleUpg = "+8 к бонусному урону.";
            skillType = Consts.SkillType.Passive;
            hero.passiveAttackDamage += Scorn;
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
                dmg += 8;
                hero.passiveAttackDamage -= Scorn;
                hero.passiveAttackDamage += Scorn;
                title = $"Ваши атаки наносят дополнительно {dmg} урона за каждый отрицательный эффект на цели.";
                return true;
            }
            return false;
        }

        private int Scorn(Hero attacker, Hero? defender, GameData gameData)
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
