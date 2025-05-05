using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Models.Items.Tier4
{
    public class HornItem : Item
    {
        int dmg = 125;
        int percentDmgReduction = 40;
        public HornItem()
        {
            Name = "Horn";
            Amount = 1;
            Cost = 80;
            Description = $"Даёт сигнал лучникам на огонь на подавление. Враги в ней получают {dmg} маг. урона,\n" +
                $" теряют 1 ОД, а их урон от атак снижается на {percentDmgReduction}%";
            Level = 4;
            SellCost = 40;

            ItemType = Consts.ItemType.Active;
            Skill = new HornSkill(dmg, percentDmgReduction);
        }

        public override void CastItem(RequestData requestData)
        {
            Skill.Cast(requestData);
        }
    }

    public class HornSkill : Skill
    {
        int percentDmgReduction = 40;
        public HornSkill(int dmg, int percentDmgReduction)
        {
            name = "Horn";
            this.dmg = dmg;
            this.percentDmgReduction = percentDmgReduction;
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 0;
            range = 3;
            radius = 1;
            nonTarget = false;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new RangeAoECastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null)
            {
                foreach (var n in UtilityService.GetHexesRadius(requestData.TargetHex, radius))
                {
                    if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                    {
                        WeaknessDebuff weaknessDebuff = new WeaknessDebuff(requestData.Caster.Id, percentDmgReduction, 2);
                        n.HERO.AddEffect(weaknessDebuff);

                        n.HERO.AP -= 1;
                        if (n.HERO.AP < 0)
                            n.HERO.AP = 0;

                        AttackService.SetDamage(requestData.Caster, n.HERO, dmg, Consts.DamageType.Magic);
                    }
                }

                requestData.Caster.SpendAP(requireAP);
                coolDownNow = coolDown;
                return true;
            }
            return false;
        }
    }
}
