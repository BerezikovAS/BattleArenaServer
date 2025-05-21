using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.VampireSkills
{
    public class ThirstPSkill : PassiveSkill
    {
        int percentHeal = 30;
        public ThirstPSkill(Hero hero) : base(hero)
        {
            name = "Thirst";
            title = $"Ваши атаки восстанавливают Вам ХП в размере {percentHeal}% от нанесенного урона.";
            titleUpg = "+15% к восстановлению.";
            skillType = Consts.SkillType.Passive;
            hero.afterAttack += Thirst;
            hero.beforeAttack += ClearDmgDealed;
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
                hero.afterAttack -= Thirst;
                percentHeal += 15;
                hero.afterAttack += Thirst;
                title = $"Ваши атаки восстанавливают Вам ХП в размере {percentHeal}% от нанесенного урона.";
                return true;
            }
            return false;
        }

        private bool Thirst(Hero attacker, Hero defender, int dmg, Consts.DamageType dmgType)
        {
            int heal = (int)(Convert.ToDouble(attacker.DamageDealed * percentHeal) / 100);
            if (attacker.HP > 0)
                attacker.Heal(heal);
            return true;
        }

        private bool ClearDmgDealed(Hero attacker, Hero defender, int dmg)
        {
            attacker.DamageDealed = 0;
            return true;
        }
    }
}
