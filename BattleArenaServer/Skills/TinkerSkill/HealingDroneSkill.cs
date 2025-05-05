using BattleArenaServer.Interfaces;
using BattleArenaServer.Models.Summons;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.TinkerSkill
{
    public class HealingDroneSkill : Skill
    {
        int droneHP = 120;
        int lifeTime = 3;
        int armor = 2;
        int resist = 4;
        int attackRadius = 0;

        public HealingDroneSkill()
        {
            name = "Healing Drone";
            dmg = 0;
            title = $"Запускает лечащего дрона под Вашим контролем.";
            titleUpg = "+20 к лечению от дрона. +1 к дальности полета.";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            range = 2;
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
                //Запускаем дрона
                int Id = GameData._heroes.Max(x => x.Id) + 1;
                HealingDroneSummon healingDroneSummon =
                    new HealingDroneSummon(Id, requestData.Caster.Team, requestData.Caster.Id, lifeTime, droneHP, armor, resist, upgraded);
                requestData.TargetHex.SetHero(healingDroneSummon);
                GameData._heroes.Add(healingDroneSummon);

                //Обновим ауры
                AttackService.ContinuousAuraAction();

                requestData.Caster.SpendAP(requireAP);
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
                return true;
            }
            return false;
        }
    }
}
