using BattleArenaServer.Interfaces;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Models.Items.Tier4
{
    public class ScytheItem : Item
    {
        int enemyHP = 200;
        int heal = 120;
        int resist = 3;
        public ScytheItem()
        {
            Name = "Scythe";
            Amount = 1;
            Cost = 80;
            Description = $"+{resist} к сопротивлениию. Коса смерти казнит врага с менее чем {enemyHP} ХП и восстанавливает Вам {heal} ХП";
            Level = 4;
            SellCost = 40;

            ItemType = Consts.ItemType.Active;
            Skill = new ScytheSkill(enemyHP, heal);
        }

        public override void ApplyEffect(Hero hero)
        {
            hero.Resist += resist;
        }

        public override void RemoveEffect(Hero hero)
        {
            hero.Resist -= resist;
        }

        public override void CastItem(RequestData requestData)
        {
            Skill.Cast(requestData);
        }
    }

    public class ScytheSkill : Skill
    {
        int enemyHP = 200;
        int heal = 120;
        public ScytheSkill(int enemyHP, int heal)
        {
            name = "Scythe";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 0;
            range = 3;
            nonTarget = false;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.Target != null)
            {
                if (requestData.Target.HP > enemyHP)
                    return false;

                AttackService.KillHero(requestData.Target);
                requestData.Target.Heal(heal);

                requestData.Caster.AP -= requireAP;
                coolDownNow = coolDown;
                return true;
            }
            return false;
        }
    }
}
