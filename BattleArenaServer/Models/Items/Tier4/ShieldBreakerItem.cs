using BattleArenaServer.Effects;
using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Models.Items.Tier4
{
    public class ShieldBreakerItem : Item
    {
        int dmg = 125;
        int armorResistReduce = 4;
        public ShieldBreakerItem()
        {
            Name = "ShieldBreaker";
            Amount = 1;
            Cost = 80;
            Description = $"Тяжёлая булава проламывает оборону врага. Снимает все щиты, наносит {dmg} чистого урона \n" +
                $"и снижает броню и сопротивление на {armorResistReduce}";
            Level = 4;
            SellCost = 40;

            ItemType = Consts.ItemType.Active;
            Skill = new ShieldBreakerSkill(dmg, armorResistReduce);
        }

        public override void CastItem(RequestData requestData, GameData gameData)
        {
            Skill.Cast(requestData, gameData);
        }
    }

    public class ShieldBreakerSkill : Skill
    {
        int armorResistReduce = 2;
        public ShieldBreakerSkill(int dmg, int armorResistReduce)
        {
            name = "ShieldBreaker";
            this.dmg = dmg;
            this.armorResistReduce = armorResistReduce;
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 0;
            range = 1;
            nonTarget = false;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Pure;
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (!request.startRequest(requestData, this, gameData))
                return false;

            if (requestData.Caster != null && requestData.Target != null)
            {
                List<Effect> shields = requestData.Target.EffectList.FindAll(x => x.effectTags.Contains(Consts.EffectTag.PhysShield) ||
                    x.effectTags.Contains(Consts.EffectTag.MagicShield) || x.effectTags.Contains(Consts.EffectTag.DmgShield));
                foreach (var shield in shields)
                {
                    shield.RemoveEffect(requestData.Target, gameData);
                    requestData.Target.EffectList.Remove(shield);
                }

                ArmorDebuff armorDebuff = new ArmorDebuff(requestData.Caster.Id, armorResistReduce, 2);
                requestData.Target.AddEffect(armorDebuff, gameData);

                ResistDebuff resistDebuff = new ResistDebuff(requestData.Caster.Id, armorResistReduce, 2);
                requestData.Target.AddEffect(resistDebuff, gameData);

                AttackService.SetDamage(requestData.Caster, requestData.Target, dmg, dmgType, gameData);

                requestData.Caster.SpendAP(requireAP, gameData);
                coolDownNow = coolDown;
                return true;
            }
            return false;
        }
    }
}
