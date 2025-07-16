using BattleArenaServer.Effects;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Models.Items.Tier3
{
    public class HolyCleansingItem : Item
    {
        int dmg = 110;
        public HolyCleansingItem()
        {
            Name = "HolyCleansing";
            Amount = 1;
            Cost = 55;
            Description = $"Святой огонь снимает все положительные эффекты с цели и наносит ей {dmg} магического урона";
            Level = 3;
            SellCost = 27;

            ItemType = Consts.ItemType.Active;
            Skill = new HolyCleansingSkill(dmg);
        }

        public override void CastItem(RequestData requestData, GameData gameData)
        {
            Skill.Cast(requestData, gameData);
        }
    }

    public class HolyCleansingSkill : Skill
    {
        public HolyCleansingSkill(int dmg)
        {
            name = "HolyCleansing";
            this.dmg = dmg;
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 0;
            range = 2;
            nonTarget = false;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (!request.startRequest(requestData, this, gameData))
                return false;

            if (requestData.Caster != null && requestData.Target != null)
            {
                List<Effect> buffs = requestData.Target.EffectList.FindAll(x => x.type == Consts.StatusEffect.Buff);
                foreach (var buff in buffs)
                {
                    buff.RemoveEffect(requestData.Target, gameData);
                    requestData.Target.EffectList.Remove(buff);
                }

                AttackService.SetDamage(requestData.Caster, requestData.Target, dmg, dmgType, gameData);

                requestData.Caster.SpendAP(requireAP, gameData);
                coolDownNow = coolDown;
                return true;
            }
            return false;
        }
    }
}
