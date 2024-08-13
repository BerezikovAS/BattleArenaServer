using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.ChaosSkills
{
    public class ChaosStrikeSkill : Skill
    {
        int extraDmg = 0;
        public ChaosStrikeSkill()
        {
            name = "Chaos Strike";
            dmg = 165;
            title = $"Атакуйте врага, нанеся {dmg} урона случайного типа.";
            titleUpg = $"Дополнительный урон от 0 до {extraDmg}.";
            coolDown = 2;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 1;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Pure;
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.Target != null && requestData.TargetHex != null)
            {
                Random rnd = new Random();
                Consts.DamageType damageType = Consts.DamageType.Physical;
                switch (rnd.Next(0, 3))
                {
                    case 1:
                        damageType = Consts.DamageType.Magic;
                        break;
                    case 2:
                        damageType = Consts.DamageType.Pure;
                        break;
                }

                AttackService.SetDamage(requestData.Caster, requestData.Target, dmg + rnd.Next(0, extraDmg), damageType);
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
                extraDmg += 80;
                title = $"Атакуйте врага, нанеся {dmg} урона случайного типа.\n" +
                    $"+ Дополнительный урон от 0 до {extraDmg}.";
                return true;
            }
            return false;
        }
    }
}
