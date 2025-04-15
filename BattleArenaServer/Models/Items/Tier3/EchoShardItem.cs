using BattleArenaServer.Effects;
using BattleArenaServer.Effects.Debuffs;

namespace BattleArenaServer.Models.Items.Tier3
{
    public class EchoShardItem : Item
    {
        int percentDmgReduce = 40;
        public EchoShardItem()
        {
            Name = "EchoShard";
            Amount = 1;
            Cost = 55;
            Description = $"Враги, атакуя Вас получают эффект, который снижает дальнейший урон от атак по Вам на {percentDmgReduce}%";
            Level = 3;
            SellCost = 27;
        }

        public override void ApplyEffect(Hero hero)
        {
            hero.afterReceivedAttack += AfterReceivedAttack;
        }

        public override void RemoveEffect(Hero hero)
        {
            hero.afterReceivedAttack -= AfterReceivedAttack;
        }

        private bool AfterReceivedAttack(Hero attacker, Hero defender, int dmg)
        {
            Effect? effect = attacker.EffectList.FirstOrDefault(x => x.Name == "Resonance" && x.idCaster == defender.Id);
            if (effect != null)
                effect.duration = 2;
            else
            {
                ResonanceDebuff resonanceDebuff = new ResonanceDebuff(defender.Id, percentDmgReduce, 2, defender.Name);
                attacker.AddEffect(resonanceDebuff);
            }
            return true;
        }
    }
}
