using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Models.Items.Tier3
{
    public class CombatShieldItem : Item
    {
        int dmg = 90;
        int armor = 3;
        public CombatShieldItem()
        {
            Name = "CombatShield";
            Amount = 1;
            Cost = 55;
            Description = $"Зачарованный щит отбрасывает назад врагов перед собой, нанося им {dmg} маг. урона\n" +
                $"и дарует Вам {armor} брони до конца следующего хода";
            Level = 3;
            SellCost = 27;

            ItemType = Consts.ItemType.Active;
            Skill = new CombatShieldSkill(dmg, armor);
        }

        public override void CastItem(RequestData requestData)
        {
            Skill.Cast(requestData);
        }
    }

    public class CombatShieldSkill : Skill
    {
        int armor = 3;
        public CombatShieldSkill(int dmg, int armor)
        {
            name = "CombatShield";
            this.dmg = dmg;
            this.armor = armor;
            coolDown = 3;
            coolDownNow = 0;
            requireAP = 0;
            range = 1;
            radius = 1;
            nonTarget = false;
            area = Consts.SpellArea.Conus;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new LineCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null && requestData.CasterHex != null)
            {
                Hex direction = UtilityService.GetDirection(requestData.CasterHex, requestData.TargetHex);
                foreach (var n in UtilityService.GetHexesCone(requestData.CasterHex, requestData.TargetHex, radius))
                {
                    if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                    {
                        AttackService.SetDamage(requestData.Caster, n.HERO, dmg, dmgType);

                        //Гекс на который нужно оттолкнуть врага
                        Hex? targetHex = UtilityService.GetOneHexOnDirection(n, direction, 1, 1);

                        if (targetHex != null && targetHex.IsFree() && n.HERO != null && n.HERO.HP > 0)
                            AttackService.MoveHero(n.HERO, n, targetHex);
                    }
                }

                ArmorBuff buffArmor = new ArmorBuff(requestData.Caster.Id, armor, 2);
                requestData.Caster.AddEffect(buffArmor);

                requestData.Caster.AP -= requireAP;
                coolDownNow = coolDown;
                return true;
            }
            return false;
        }
    }
}
