using BattleArenaServer.Effects.Unique;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Models.Summons;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.ShadowSkills
{
    public class ShadowTwinSkill : Skill
    {
        int lifeTime = 2;
        public ShadowTwinSkill()
        {
            name = "Shadow Twin";
            dmg = 0;
            title = $"Призывает своего двойника, который обладает пассивными способностями своего заклинателя.";
            titleUpg = "-2 к перезарядке";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            range = 2;
            nonTarget = false;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Physical;
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.Target != null && requestData.TargetHex != null)
            {
                //Находим рандомную свободную клетку рядом с целью
                Hex? spawnHex = UtilityService.GetRandomAdjacentHex(requestData.TargetHex);
                if (spawnHex == null)
                    return false; //Если таковой нет, то выходим без применения эффектов

                //Создаем копию цели
                int Id = GameData._hexes.Max(x => x.HERO != null ? x.HERO.Id : 0) + 1;
                ShadowTwinSummon doppelganger = new ShadowTwinSummon(Id, requestData.Caster.Team, requestData.Caster.Id, lifeTime, requestData.Target);

                //Вешаем на неё уникальный эффект
                ShadowTwinUnique shadowTwinUnique = new ShadowTwinUnique(requestData.Caster.Id, 0, 99);
                doppelganger.AddEffect(shadowTwinUnique);

                //Спавним копию на поле
                spawnHex.SetHero(doppelganger);
                GameData._heroes.Add(doppelganger);

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
                coolDown -= 2;
                stats.coolDown -= 2;
                return true;
            }
            return false;
        }
    }
}
