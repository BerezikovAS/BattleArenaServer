using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.DwarfSkills
{
    public class ToughnessPSkill : PassiveSkill
    {
        double defaultDmgResist = 0;
        int percentLossHP = 3;
        int percentDmgResist = 1;
        public ToughnessPSkill(Hero hero) : base(hero)
        {
            name = "Toughness";
            title = $"С потерей ХП, растет сопротивляемость урону. За каждые {percentLossHP}% потерянного ХП Вы получаете {percentDmgResist}% сопротивления.";
            titleUpg = "+10% сопротивления урону по умолчанию";
            skillType = Consts.SkillType.Passive;
            hero.modifierAppliedDamage += ModifierAppliedDamage;
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
                hero.modifierAppliedDamage -= ModifierAppliedDamage;
                defaultDmgResist += 10;
                hero.modifierAppliedDamage += ModifierAppliedDamage;
                title = $"С потерей ХП, растет сопротивляемость урону. За каждые {percentLossHP}% потерянного ХП Вы получаете {percentDmgResist}% сопротивления." +
                    $"\nВы обладаете 10% сопротивления по умолчанию.";
                return true;
            }
            return false;
        }

        private int ModifierAppliedDamage(Hero? attacker, Hero defender, int dmg, Consts.DamageType dmgType)
        {
            double percent = defaultDmgResist / 100 + Convert.ToDouble(defender.MaxHP - defender.HP) / Convert.ToDouble(defender.MaxHP) / 3;
            return (int)(-1 * Convert.ToDouble(dmg) * percent);
        }
    }
}
