using BattleArenaServer.Interfaces;
using BattleArenaServer.Models.Summons;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;
using BattleArenaServer.Effects;

namespace BattleArenaServer.Skills.DruidSkills
{
    public class WolfSkill : Skill
    {
        int wolfHP = 200;
        int lifeTime = 4;
        int armor = 4;
        int resist = 2;
        int attackRadius = 1;

        public WolfSkill()
        {
            name = "Wolf";
            dmg = 70;
            title = $"Призывает спутника волка, сражающегося под Вашим контролем.";
            titleUpg = "Атаки волка снимают случайный положительный эффект с цели.";
            coolDown = 5;
            coolDownNow = 0;
            requireAP = 2;
            range = 1;
            nonTarget = false;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Physical;
        }

        public new ISkillCastRequest request => new HexTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null && requestData.TargetHex.OBSTACLE == null)
            {
                //Вызываем скелета
                int Id = GameData._heroes.Max(x => x.Id) + 1;
                WolfSummon wolf = new WolfSummon(Id, requestData.Caster.Team, wolfHP, armor, resist, attackRadius, dmg, requestData.Caster.Id, lifeTime);
                requestData.TargetHex.SetHero(wolf);
                GameData._heroes.Add(wolf);

                if (upgraded)
                    wolf.afterAttack += AfterAttackDelegate;

                //Обновим ауры
                AttackService.ContinuousAuraAction();

                requestData.Caster.AP -= requireAP;
                coolDownNow = coolDown;
                return true;
            }
            return false;
        }

        public override bool UpgradeSkill()
        {
            if (!upgraded)
            {
                upgraded = true;
                title = $"Призывает спутника волка, сражающегося под Вашим контролем. Атаки волка снимают случайный положительный эффект с цели.";
                return true;
            }
            return false;
        }

        private bool AfterAttackDelegate(Hero attacker, Hero? defender, int dmg, Consts.DamageType dmgType)
        {
            if (defender != null)
            {
                List<Effect> buffs = defender.EffectList.FindAll(x => x.type == Consts.StatusEffect.Buff);
                if (buffs.Count() > 0)
                {
                    Random rnd = new Random();
                    Effect buff = buffs[rnd.Next(buffs.Count())];
                    buff.RemoveEffect(defender);
                    defender.EffectList.Remove(buff);
                }
            }

            return true;
        }
    }
}
