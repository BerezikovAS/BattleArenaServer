using BattleArenaServer.Interfaces;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Models.Items.Tier1
{
    public class FeatherItem : Item
    {
        int heal = 14;
        int maxCharges = 10;

        public FeatherItem()
        {
            Name = "Feather";
            Amount = 1;
            Cost = 10;
            Description = $"Когда Вы используете действие перемещения, перо получает 1 заряд. Используйте перо, чтобы восстановить {heal} ХП за заряд. (Макс: {maxCharges} зарядов)";
            Level = 1;
            SellCost = 5;

            ItemType = Consts.ItemType.Active;
            Skill = new FeatherSkill(heal, maxCharges);
        }

        public override void CastItem(RequestData requestData)
        {
            Skill.Cast(requestData);
        }

        public override void ApplyEffect(Hero hero)
        {
            hero.afterMove += AfterMove;
        }

        public override void RemoveEffect(Hero hero)
        {
            (Skill as FeatherSkill).charges = 0;
            hero.afterMove -= AfterMove;
            Description = $"Когда Вы используете действие перемещения, перо получает 1 заряд. Используйте перо, чтобы восстановить {heal} ХП за заряд. (Макс: 10 зарядов)";

        }

        private void AfterMove(Hero hero, Hex? currentHex, Hex targetHex)
        {
            FeatherSkill feather = Skill as FeatherSkill;
            feather.AddCharge();
            Description = $"Когда Вы используете действие перемещения, перо получает 1 заряд. Используйте перо, чтобы восстановить {heal} ХП за заряд. (Макс: 10 зарядов)" +
                $"\nНакоплено {feather.charges} зарядов.";
        }
    }

    public class FeatherSkill : Skill
    {
        int heal = 14;
        int maxCharges = 10;
        public int charges { get; set; } = 0;

        public FeatherSkill(int heal, int maxCharges)
        {
            name = "Feather";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 0;
            nonTarget = true;
            area = Consts.SpellArea.NonTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            this.heal = heal;
            this.maxCharges = maxCharges;
        }

        public new ISkillCastRequest request => new NontargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (request.startRequest(requestData, this))
            {
                if (requestData.Caster != null)
                {
                    requestData.Caster.Heal(heal * charges);
                    charges = 0;

                    requestData.Caster.AP -= requireAP;
                    coolDownNow = coolDown;
                    return true;
                }
            }
            return false;
        }

        public void AddCharge()
        {
            charges = charges < maxCharges ? charges + 1 : maxCharges;
        }
    }
}
