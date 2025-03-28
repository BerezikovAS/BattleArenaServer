using BattleArenaServer.Effects;
using BattleArenaServer.Effects.Unique;
using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Skills.WitchDoctorSkills.Auras
{
    public class WitchTotemAura : Aura
    {
        int baseDmg;
        public WitchTotemAura(int baseDmg)
        {
            Name = "WitchTotemAura";
            radius = 2;
            type = Consts.AuraType.EndTurn;
            this.baseDmg = baseDmg;
        }

        public override void ApplyEffect(Hero source, Hero target)
        {
            if (source.Team != target.Team && target.type != Consts.HeroType.Obstacle)
            {
                Effect? totemCharges = source.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.TotemCharge));
                if (totemCharges != null)
                {
                    totemCharges.ApplyEffect(target);
                    AttackService.SetDamage(source, target, baseDmg, Consts.DamageType.Pure);
                }
                else
                {
                    TotemChargeUnique totemChargeUnique = new TotemChargeUnique(source.Id, 1, 99);
                    source.AddEffect(totemChargeUnique);
                    AttackService.SetDamage(source, target, baseDmg, Consts.DamageType.Pure);
                }
            }
        }

        public override void CancelEffect(Hero source)
        {

        }
    }
}
