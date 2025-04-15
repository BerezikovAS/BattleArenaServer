using BattleArenaServer.Interfaces;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Models.Items.Tier3
{
    public class HammerItem : Item
    {
        int dmg = 150;
        public HammerItem()
        {
            Name = "Hammer";
            Amount = 1;
            Cost = 55;
            Description = $"Мощный удар сверху наносит {dmg} физ. урона и отнмает 1 ОД у врага";
            Level = 3;
            SellCost = 27;

            ItemType = Consts.ItemType.Active;
            Skill = new HammerSkill(dmg);
        }

        public override void CastItem(RequestData requestData)
        {
            Skill.Cast(requestData);
        }
    }

    public class HammerSkill : Skill
    {
        public HammerSkill(int dmg)
        {
            name = "ThrowingAxe";
            this.dmg = dmg;
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 0;
            range = 1;
            nonTarget = false;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Physical;
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.Target != null)
            {
                AttackService.SetDamage(requestData.Caster, requestData.Target, dmg, dmgType);
                requestData.Target.AP -= 1;

                if (requestData.Target.AP < 0)
                    requestData.Target.AP = 0;

                requestData.Caster.AP -= requireAP;
                coolDownNow = coolDown;
                return true;
            }
            return false;
        }
    }
}
