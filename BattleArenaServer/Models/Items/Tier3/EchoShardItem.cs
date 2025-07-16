using BattleArenaServer.Effects;
using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Services;

namespace BattleArenaServer.Models.Items.Tier3
{
    public class EchoShardItem : Item
    {
        int percentDmgReduce = 40;
        int percentDmgReturn = 15;
        public EchoShardItem()
        {
            Name = "EchoShard";
            Amount = 1;
            Cost = 55;
            Description = $"Враги, атакуя Вас получают {percentDmgReturn}% от нанесенного урона и эффект, который снижает дальнейший урон от атак по Вам на {percentDmgReduce}%";
            Level = 3;
            SellCost = 27;
        }

        public override void ApplyEffect(Hero hero, GameData gameData)
        {
            hero.afterReceivedAttack += AfterReceivedAttack;
        }

        public override void RemoveEffect(Hero hero, GameData gameData)
        {
            hero.afterReceivedAttack -= AfterReceivedAttack;
        }

        private bool AfterReceivedAttack(Hero attacker, Hero defender, int dmg, GameData gameData)
        {
            Effect? effect = attacker.EffectList.FirstOrDefault(x => x.Name == "Resonance" && x.idCaster == defender.Id);
            if (effect != null)
                effect.duration = 2;
            else
            {
                ResonanceDebuff resonanceDebuff = new ResonanceDebuff(defender.Id, percentDmgReduce, 2, defender.Name);
                attacker.AddEffect(resonanceDebuff, gameData);
            }

            int returnDmg = (int)(Convert.ToDouble(dmg * percentDmgReturn) / 100);
            AttackService.SetDamage(defender, attacker, returnDmg, Consts.DamageType.Physical, gameData);

            return true;
        }
    }
}
