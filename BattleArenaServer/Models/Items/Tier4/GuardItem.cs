using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Models.Items.Tier4
{
    public class GuardItem : Item
    {
        int heal = 100;
        int shieldDurability = 125;
        public GuardItem()
        {
            Name = "Guard";
            Amount = 1;
            Cost = 80;
            Description = $"Вы и союзники в радиусе 2-х клеток восстанавливаете {heal} ХП и получаете щит, блокирующий {shieldDurability} урона";
            Level = 4;
            SellCost = 40;

            ItemType = Consts.ItemType.Active;
            Skill = new GuardSkill(heal, shieldDurability);
        }

        public override void CastItem(RequestData requestData)
        {
            Skill.Cast(requestData);
        }
    }

    public class GuardSkill : Skill
    {
        int heal = 100;
        int shieldDurability = 125;
        public GuardSkill(int heal, int shieldDurability)
        {
            name = "Guard";
            this.heal = heal;
            this.shieldDurability = shieldDurability;
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 0;
            nonTarget = false;
            radius = 2;
            range = 0;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new NonTargerAoECastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (request.startRequest(requestData, this))
            {
                if (requestData.Caster != null && requestData.TargetHex != null)
                {
                    foreach (var n in UtilityService.GetHexesRadius(requestData.TargetHex, radius))
                    {
                        if (n.HERO != null && n.HERO.Team == requestData.Caster.Team)
                        {
                            n.HERO.Heal(heal);

                            DmgShieldBuff dmgShieldBuff = new DmgShieldBuff(requestData.Caster.Id, shieldDurability, 2);
                            n.HERO.AddEffect(dmgShieldBuff);
                        }
                    }
                    requestData.Caster.SpendAP(requireAP);
                    coolDownNow = coolDown;
                    return true;
                }
            }
            return false;
        }
    }
}
