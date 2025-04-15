using BattleArenaServer.Interfaces;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Models.Items.Tier1
{
    public class GrapplingHookItem : Item
    {
        int dmg = 50;
        public GrapplingHookItem()
        {
            Name = "GrapplingHook";
            Amount = 1;
            Cost = 10;
            Description = $"Притягивает первого попавшегося врага на линии и наносит тому {dmg} маг. урона";
            Level = 1;
            SellCost = 5;

            ItemType = Consts.ItemType.Active;
            Skill = new GrapplingHookSkill(dmg);
        }

        public override void CastItem(RequestData requestData)
        {
            Skill.Cast(requestData);
        }
    }

    public class GrapplingHookSkill : Skill
    {
        public GrapplingHookSkill(int dmg)
        {
            name = "GrapplingHook";
            this.dmg = dmg;
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 0;
            range = 2;
            radius = 2;
            nonTarget = false;
            area = Consts.SpellArea.Line;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new LineCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.CasterHex != null && requestData.TargetHex != null)
            {
                int pos = 1;
                Hero? target = null;
                Hex? targetHex = null;
                while (pos <= radius && target == null)
                {
                    targetHex = UtilityService.GetOneHexOnDirection(requestData.CasterHex, requestData.TargetHex, pos);
                    target = targetHex?.HERO?.Team != requestData.Caster.Team ? targetHex?.HERO : null;
                    pos++;
                }

                if (target != null && targetHex != null)
                {
                    Hex? hex = UtilityService.GetOneHexOnDirection(targetHex, requestData.CasterHex, 1);
                    if (hex != null && hex.IsFree())
                        AttackService.MoveHero(target, targetHex, hex);

                    AttackService.SetDamage(requestData.Caster, target, dmg, dmgType);
                }

                requestData.Caster.AP -= requireAP;
                coolDownNow = coolDown;
                return true;
            }
            return false;
        }
    }
}
