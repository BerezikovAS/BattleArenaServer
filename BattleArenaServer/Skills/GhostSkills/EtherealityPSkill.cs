using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.GhostSkills
{
    public class EtherealityPSkill : PassiveSkill
    {
        int percentReduce = 50;
        public EtherealityPSkill(Hero hero) : base(hero)
        {
            name = "Ethereality";
            title = $"Ваше тело бесплотно и Вы получаете на {percentReduce}% меньше физического урона.";
            titleUpg = "+20% к уменьшению физического урона";
            skillType = Consts.SkillType.Passive;
            hero.modifierAppliedDamage += ModifierAppliedDmg;
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
                hero.modifierAppliedDamage -= ModifierAppliedDmg;
                percentReduce += 20;
                hero.modifierAppliedDamage += ModifierAppliedDmg;
                title = $"Ваше тело бесплотно и Вы получаете на {percentReduce}% меньше физического урона.";
                return true;
            }
            return false;
        }

        private int ModifierAppliedDmg(Hero? attacker, Hero defender, int dmg, Consts.DamageType dmgType)
        {
            int percent = 0;
            if (dmgType == Consts.DamageType.Physical)
                percent = percentReduce;

            return (int)(-1 * Convert.ToDouble(dmg) * Convert.ToDouble(percent) / 100);
        }
    }
}
