using BattleArenaServer.Effects;
using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.AssassinSkills
{
    public class LiquidationPSkill : PassiveSkill
    {
        int extraDmg = 6;
        public LiquidationPSkill(Hero hero) : base(hero)
        {
            name = "Liquidation";
            title = $"Концентрируясь на одной цели, Вы наносите ей больше урона от атак. Ваши атаки накладывают эффект на врага." +
                $"Цель теряет {extraDmg} ХП, за каждый стак эффекта. Атакуя другого врага, все остальные герои теряют накопленные стаки.";
            titleUpg = "Цель теряет по 9 ХП за стак.";
            skillType = Consts.SkillType.Passive;
            hero.beforeAttack += Liquidation;
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
                hero.beforeAttack -= Liquidation;
                extraDmg += 3;
                hero.beforeAttack += Liquidation;
                title = $"Концентрируясь на одной цели, Вы наносите ей больше урона от атак. Ваши атаки накладывают эффект на врага." +
                    $"Цель теряет {extraDmg} ХП, за каждый стак эффекта. Атакуя другого врага, все остальные герои теряют накопленные стаки.";
                return true;
            }
            return false;
        }

        private bool Liquidation(Hero attacker, Hero defender, int dmg)
        {
            Effect? liquidation = defender.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Liquidation));
            if (liquidation == null)
            {
                foreach (var hero in GameData._heroes.Where(x => x.Team != attacker.Team))
                {
                    Effect? liquidationRemove = hero.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Liquidation));
                    if (liquidationRemove != null)
                    {
                        liquidationRemove.RemoveEffect(hero);
                        hero.EffectList.Remove(liquidationRemove);
                    }
                }

                LiquidationDebuff liquidationDebuff = new LiquidationDebuff(attacker.Id, extraDmg, 99);
                defender.AddEffect(liquidationDebuff);
            }
            else
            {
                liquidation.RemoveEffect(defender);
                liquidation.value += extraDmg;
                liquidation.ApplyEffect(defender);
            }
            return true;
        }
    }
}
