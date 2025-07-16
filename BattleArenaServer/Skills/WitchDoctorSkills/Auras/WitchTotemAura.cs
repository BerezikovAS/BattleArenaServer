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

        public override void ApplyEffect(Hero source, Hero target, GameData gameData)
        {
            if (source.Team != target.Team && target.type != Consts.HeroType.Obstacle)
            {
                Effect? totemCharges = source.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.TotemCharge));
                if (totemCharges != null)
                {
                    totemCharges.ApplyEffect(target, gameData);
                    AttackService.SetDamage(source, target, baseDmg, Consts.DamageType.Pure, gameData);
                }
                else
                {
                    TotemChargeUnique totemChargeUnique = new TotemChargeUnique(source.Id, 1, 99);
                    source.AddEffect(totemChargeUnique, gameData);
                    AttackService.SetDamage(source, target, baseDmg, Consts.DamageType.Pure, gameData);
                }
            }
        }

        public override void CancelEffect(Hero source, GameData gameData)
        {

        }
    }
}
