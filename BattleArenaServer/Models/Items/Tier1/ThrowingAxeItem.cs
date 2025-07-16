using BattleArenaServer.Interfaces;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Models.Items.Tier1
{
    public class ThrowingAxeItem : Item
    {
        int extraDmg = 15;
        public ThrowingAxeItem()
        {
            Name = "ThrowingAxe";
            Amount = 1;
            Cost = 10;
            Description = $"Проведите атаку на расстоянии до 2-х клеток с бонусным уроном +{extraDmg}";
            Level = 1;
            SellCost = 5;

            ItemType = Consts.ItemType.Active;
            Skill = new ThrowingAxeSkill(extraDmg);
        }

        public override void CastItem(RequestData requestData, GameData gameData)
        {
            Skill.Cast(requestData, gameData);
        }
    }

    public class ThrowingAxeSkill : Skill
    {
        int extraDmg = 0;
        public ThrowingAxeSkill(int extraDmg)
        {
            name = "ThrowingAxe";
            this.extraDmg = extraDmg;
            coolDown = 2;
            coolDownNow = 0;
            requireAP = 0;
            range = 2;
            nonTarget = false;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Physical;
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (!request.startRequest(requestData, this, gameData))
                return false;

            if (requestData.Caster != null && requestData.Target != null)
            {
                requestData.Caster.Dmg += extraDmg;
                requestData.Caster.AttackRadius += 1;

                bool success = AttackService.AttackHero(requestData, gameData);

                requestData.Caster.Dmg -= extraDmg;
                requestData.Caster.AttackRadius -= 1;

                if (success)
                    coolDownNow = coolDown;
                return true;
            }
            return false;
        }
    }
}
