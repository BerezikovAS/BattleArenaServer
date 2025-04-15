using BattleArenaServer.Effects.Buffs;

namespace BattleArenaServer.Models.Items.Auras
{
    public class BubbleAura : Aura
    {
        public BubbleAura()
        {
            Name = "BubbleAura";
            radius = 0;
            type = Consts.AuraType.StartTurn;
        }

        public override void ApplyEffect(Hero source, Hero target)
        {
            if (source.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Bubble)) == null)
            {
                BubbleBuff bubbleBuff = new BubbleBuff(source.Id, 0, 99);
                source.AddEffect(bubbleBuff);
            }
        }

        public override void CancelEffect(Hero source)
        {

        }
    }
}
