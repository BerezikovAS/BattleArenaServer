using BattleArenaServer.Models;
using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Effects;

namespace BattleArenaServer.Skills.SeraphimSkills
{
    public class SwiftnessPSkill : PassiveSkill
    {
        public SwiftnessPSkill(Hero hero) : base(hero)
        {
            name = "Swiftness";
            title = $"В начале хода Вы получаете ускорение.";
            titleUpg = "Также снимает эффекты замедления и обездвиживания.";

            HasteBuff hasteBuff = new HasteBuff(hero.Id, 0, 1);
            hero.AddEffect(hasteBuff);
        }

        public override bool Cast(RequestData requestData)
        {
            return false;
        }

        public override void refreshEffect()
        {
            if (hero.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Haste)) == null)
            {
                HasteBuff hasteBuff = new HasteBuff(hero.Id, 0, 1);
                hero.AddEffect(hasteBuff);
            }

            if (upgraded)
            {
                List<Effect> removeEffects = new List<Effect>();
                foreach (Effect effect in hero.EffectList)
                {
                    if (effect.effectTags.Contains(Consts.EffectTag.Slow) || effect.effectTags.Contains(Consts.EffectTag.Root))
                    {
                        removeEffects.Add(effect);
                        effect.RemoveEffect(hero);
                    }
                }
                foreach (Effect effect in removeEffects)
                    hero.EffectList.Remove(effect);
            }
        }

        public override bool UpgradeSkill()
        {
            upgraded = true;
            title = $"В начале хода Вы получаете ускорение, а также снимаете эффекты замедления и обездвиживания.";
            return true;
        }
    }
}
