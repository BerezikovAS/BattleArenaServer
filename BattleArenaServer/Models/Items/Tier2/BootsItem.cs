using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Models.Items.Tier2
{
    public class BootsItem : Item
    {
        public BootsItem()
        {
            Name = "Boots";
            Amount = 1;
            Cost = 25;
            Description = $"Дарует ускорение";
            Level = 2;
            SellCost = 12;

            ItemType = Consts.ItemType.Active;
            Skill = new BootsSkill();
        }

        public override void CastItem(RequestData requestData, GameData gameData)
        {
            Skill.Cast(requestData, gameData);
        }
    }

    public class BootsSkill : Skill
    {
        public BootsSkill()
        {
            name = "Boots";
            coolDown = 2;
            coolDownNow = 0;
            requireAP = 0;
            nonTarget = true;
            area = Consts.SpellArea.NonTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new NontargetCastRequest();

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (request.startRequest(requestData, this, gameData))
            {
                if (requestData.Caster != null)
                {
                    HasteBuff hasteBuff = new HasteBuff(requestData.Caster.Id, 0, 1);
                    requestData.Caster.AddEffect(hasteBuff, gameData);

                    coolDownNow = coolDown;
                    return true;
                }
            }
            return false;
        }
    }
}
