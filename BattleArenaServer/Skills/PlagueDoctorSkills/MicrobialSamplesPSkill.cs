using BattleArenaServer.Effects;
using BattleArenaServer.Effects.Unique;
using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Skills.PlagueDoctorSkills
{
    public class MicrobialSamplesPSkill : PassiveSkill
    {
        public MicrobialSamplesPSkill(Hero hero) : base(hero)
        {
            name = "Microbial Samples";
            title = $"В начале своего хода собирает образцы микробов с соседних существ. При достижении определенных порогов Вы увеличиваете свои показатели.";
            titleUpg = "Снижает порог образцов на 3";
        }

        public override bool Cast(RequestData requestData)
        {
            return false;
        }

        public override void refreshEffect()
        {
            int addSamples = 0;
            Hex? hex = GameData._hexes.FirstOrDefault(x => x.HERO != null && x.HERO.Id == hero.Id);
            if (hex != null)
            {
                foreach (var n in UtilityService.GetHexesRadius(hex, 1))
                {
                    if (n.HERO != null && n.HERO.Id != hero.Id)
                        addSamples++;
                }

                if (addSamples <= 0)
                    return;

                Effect? effect = hero.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.MicrobialSamples));
                if (effect != null)
                {
                    effect.value += addSamples;
                    effect.ApplyEffect(hero);
                }
                else
                {
                    MicrobialSamplesUnique microbialSamplesUnique = new MicrobialSamplesUnique(hero.Id, addSamples, 9999, upgraded);
                    hero.AddEffect(microbialSamplesUnique);
                }
            }
        }

        public override bool UpgradeSkill()
        {
            upgraded = true;
            Effect? effect = hero.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.MicrobialSamples));
            if (effect != null)
                effect.ApplyEffect(hero);

            return true;
        }
    }
}
