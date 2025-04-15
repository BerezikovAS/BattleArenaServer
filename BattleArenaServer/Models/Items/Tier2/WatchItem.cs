using BattleArenaServer.Interfaces;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Models.Items.Tier2
{
    public class WatchItem : Item
    {
        public WatchItem()
        {
            Name = "Watch";
            Amount = 1;
            Cost = 25;
            Description = $"Уменьшает время перезарядки всех способностей на 1 и уходит на перезаярку равную кол-ву затронутых способностей + 1.";
            Level = 2;
            SellCost = 12;

            ItemType = Consts.ItemType.Active;
            Skill = new WatchSkill();
        }

        public override void CastItem(RequestData requestData)
        {
            Skill.Cast(requestData);
        }
    }

    public class WatchSkill : Skill
    {
        public WatchSkill()
        {
            name = "Watch";
            coolDown = 99;
            coolDownNow = 0;
            requireAP = 0;
            nonTarget = true;
            area = Consts.SpellArea.NonTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new NontargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (request.startRequest(requestData, this))
            {
                if (requestData.Caster != null)
                {
                    int skillCount = 0;
                    foreach (var skill in requestData.Caster.SkillList)
                        if (skill.coolDownNow > 0)
                        {
                            skill.coolDownNow--;
                            skillCount++;
                        }

                    if (skillCount > 0)
                        coolDownNow = skillCount + 1;

                    return true;
                }
            }
            return false;
        }
    }
}
