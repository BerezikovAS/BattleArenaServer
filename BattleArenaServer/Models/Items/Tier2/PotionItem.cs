using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Models.Items.Tier2
{
    public class PotionItem : Item
    {
        int heal = 65;
        int regen = 30;

        public PotionItem()
        {
            Name = "Potion";
            Amount = 1;
            Cost = 25;
            Description = $"Восстанавливает {heal} ХП и даёт регенрацию в {regen} ХП на 2 хода.";
            Level = 2;
            SellCost = 12;

            ItemType = Consts.ItemType.Active;
            Skill = new PotionSkill(heal, regen);
        }

        public override void CastItem(RequestData requestData)
        {
            Skill.Cast(requestData);
        }
    }

    public class PotionSkill : Skill
    {
        int heal;
        int regen;

        public PotionSkill(int heal, int regen)
        {
            name = "Potion";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 0;
            nonTarget = true;
            area = Consts.SpellArea.NonTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            this.heal = heal;
            this.regen = regen;
        }

        public new ISkillCastRequest request => new NontargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (request.startRequest(requestData, this))
            {
                if (requestData.Caster != null)
                {
                    requestData.Caster.Heal(heal);

                    RegenerationBuff regenerationBuff = new RegenerationBuff(requestData.Caster.Id, regen, 2);
                    requestData.Caster.AddEffect(regenerationBuff);

                    requestData.Caster.AP -= requireAP;
                    coolDownNow = coolDown;
                    return true;
                }
            }
            return false;
        }
    }
}
