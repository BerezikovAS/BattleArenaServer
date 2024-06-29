using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;
using static BattleArenaServer.Models.Consts;

namespace BattleArenaServer.Skills.Knight
{
    public class ShieldBashSkill : BodyGuardSkill
    {
        int extraDmg = 20;
        int loseAP = 1;
        public ShieldBashSkill()
        {
            name = "Shieldbash";
            dmg = 60;
            title = $"Оглушает врага и наносит ему физический урон, зависящий от брони владельца.\nОглушенный враг теряет {loseAP} ОД в свой ход. ({extraDmg} доп. урона за ед. брони)";
            titleUpg = "+10 к урону за броню, +1 потере ОД";
            coolDown = 4;
            coolDownNow = 0;
            range = 1;
            requireAP = 2;
            nonTarget = false;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = DamageType.Physical;
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Target != null && requestData.Caster != null)
            {
                requestData.Caster.AP -= requireAP;
                requestData.Target.AP -= loseAP;
                AttackService.SetDamage(requestData.Caster, requestData.Target, dmg + extraDmg * requestData.Caster.Armor, dmgType);
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
                loseAP += 1;
                extraDmg += 10;
                title = $"Оглушает врага и наносит ему физический урон, зависящий от брони владельца.\nОглушенный враг теряет {loseAP} ОД в свой ход. ({extraDmg} доп. урона за ед. брони)";
                return true;
            }
            return false;
        }
    }
}
