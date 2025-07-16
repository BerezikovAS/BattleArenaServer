using BattleArenaServer.Interfaces;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Models.Items.Tier2
{
    public class WatchItem : Item
    {
        int resistPiercing = 1;
        public WatchItem()
        {
            Name = "Watch";
            Amount = 1;
            Cost = 25;
            Description = $"Атаки и способности игнорируют {resistPiercing} сопротивления.\n" + 
                $"Уменьшает время перезарядки всех способностей на 1 и уходит на перезаярку равную кол-ву затронутых способностей + 1.";
            Level = 2;
            SellCost = 12;

            ItemType = Consts.ItemType.Active;
            Skill = new WatchSkill();
        }

        public override void ApplyEffect(Hero hero, GameData gameData)
        {
            hero.resistPiercing += ResistPiercingDelegate;
        }

        public override void RemoveEffect(Hero hero, GameData gameData)
        {
            hero.resistPiercing -= ResistPiercingDelegate;
        }

        public override void CastItem(RequestData requestData, GameData gameData)
        {
            Skill.Cast(requestData, gameData);
        }

        private int ResistPiercingDelegate(Hero attacker, Hero defender)
        {
            return resistPiercing;
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

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (request.startRequest(requestData, this, gameData))
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
