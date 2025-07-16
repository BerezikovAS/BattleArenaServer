using BattleArenaServer.Interfaces;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Models.Items.Tier1
{
    public class ScrollItem : Item
    {
        public ScrollItem()
        {
            Name = "Scroll";
            Amount = 1;
            Cost = 10;
            Description = $"Телепортирует Вас к союзнику на ближайшую клетку";
            Level = 1;
            SellCost = 5;

            ItemType = Consts.ItemType.Active;
            Skill = new ScrollSkill();
        }

        public override void CastItem(RequestData requestData, GameData gameData)
        {
            Skill.Cast(requestData, gameData);
        }
    }

    public class ScrollSkill : Skill
    {
        public ScrollSkill()
        {
            name = "Scroll";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 0;
            range = 99;
            nonTarget = false;
            area = Consts.SpellArea.FriendTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new FriendTargetCastRequest();

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (!request.startRequest(requestData, this, gameData))
                return false;

            if (requestData.Caster != null && requestData.CasterHex != null && requestData.Target != null && requestData.TargetHex != null)
            {
                List<Hex> emptyHexes = UtilityService.GetHexesRadius(requestData.TargetHex, 1, gameData).Where(x => x.IsFree()).ToList();
                if (emptyHexes.Count == 0)
                    return false;

                //Ищем случайный ближайший гекс рядом с целью
                int minDist = emptyHexes.Min(x => x.Distance(requestData.CasterHex));
                List<Hex> nearestHexes = emptyHexes.Where(x => x.Distance(requestData.CasterHex) == minDist).ToList();
                Random rnd = new Random();
                Hex moveHex = nearestHexes[rnd.Next(nearestHexes.Count)];

                //Телепортируемся поближе к союзнику
                AttackService.MoveHero(requestData.Caster, requestData.CasterHex, moveHex, gameData);

                requestData.Caster.SpendAP(requireAP, gameData);
                coolDownNow = coolDown;
                return true;
            }

            return false;
        }
    }
}
