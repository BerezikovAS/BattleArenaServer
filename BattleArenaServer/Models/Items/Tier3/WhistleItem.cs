using BattleArenaServer.Interfaces;
using BattleArenaServer.Models.Summons;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Models.Items.Tier3
{
    public class WhistleItem : Item
    {
        public WhistleItem()
        {
            Name = "Whistle";
            Amount = 1;
            Cost = 55;
            Description = $"Призывает на помощь союзного орла";
            Level = 3;
            SellCost = 27;

            ItemType = Consts.ItemType.Active;
            Skill = new WhistleSkill();
        }

        public override void CastItem(RequestData requestData)
        {
            Skill.Cast(requestData);
        }
    }

    public class WhistleSkill : Skill
    {
        int eagleHP = 175;
        int armor = 3;
        int resist = 3;
        int eagleDmg = 65;
        int lifeTime = 3;

        public WhistleSkill()
        {
            name = "Whistle";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 0;
            range = 1;
            nonTarget = false;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new HexTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null && requestData.TargetHex.OBSTACLE == null)
            {
                //Вызываем орла
                int Id = GameData._heroes.Max(x => x.Id) + 1;
                EagleSummon eagle = new EagleSummon(Id, requestData.Caster.Team, requestData.Caster.Id, lifeTime, eagleHP, armor, resist, eagleDmg);
                requestData.TargetHex.SetHero(eagle);
                GameData._heroes.Add(eagle);

                //Обновим ауры
                AttackService.ContinuousAuraAction();

                requestData.Caster.SpendAP(requireAP);
                coolDownNow = coolDown;
                return true;
            }
            return false;
        }
    }
}
