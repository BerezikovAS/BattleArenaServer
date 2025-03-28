using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Models.Summons;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.TinkerSkill
{
    public class FlameTurretSkill : Skill
    {
        int turretHP = 150;
        int lifeTime = 3;
        int armor = 5;
        int resist = 5;
        int attackRadius = 0;

        public FlameTurretSkill()
        {
            name = "Flame Turret";
            dmg = 0;
            title = $"Устанавливает огнеметную туррель под Вашим контролем.";
            titleUpg = "Добавляет возможность атаки по одной цели.";
            coolDown = 4;
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
                //Ставим туррель
                int Id = GameData._hexes.Max(x => x.HERO != null ? x.HERO.Id : 0) + 1;
                FlameTurretSummon flameTurretSummon = 
                    new FlameTurretSummon(Id, requestData.Caster.Team, requestData.Caster.Id, lifeTime, turretHP, armor, resist, upgraded);
                requestData.TargetHex.SetHero(flameTurretSummon);
                GameData._heroes.Add(flameTurretSummon);

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
                return true;
            }
            return false;
        }
    }
}
