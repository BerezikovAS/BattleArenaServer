using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Services;

namespace BattleArenaServer.Models.Items.Auras
{
    public class CauldronAura : Aura
    {
        int dmg = 70;
        int percentLoss = 10;
        public CauldronAura(int dmg, int percentLoss)
        {
            Name = "CauldronAura";
            radius = 1;
            type = Consts.AuraType.EndTurn;
            this.dmg = dmg;
            this.percentLoss = percentLoss;
        }

        public override void ApplyEffect(Hero source, Hero target)
        {
            if (source.Team != target.Team)
            {
                PoisonDebuff poisonDebuff = new PoisonDebuff(source.Id, percentLoss, 2);
                target.AddEffect(poisonDebuff);

                AttackService.SetDamage(source, target, dmg, Consts.DamageType.Magic);
            }
        }

        public override void CancelEffect(Hero source)
        {

        }
    }
}
