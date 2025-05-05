using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Models.Obstacles;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.AeroturgSkills
{
    public class SwapSkill : Skill
    {
        int extraArmor = 3;
        public SwapSkill()
        {
            name = "Swap";
            title = $"Поменяйтесь местами с любым другим героем. Если это был союзник, то он получает 3 брони, если враг то теряет 3 брони.";
            titleUpg = "+1 к дальности, -2 к перезарядке";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            nonTarget = false;
            range = 3;
            area = Consts.SpellArea.HeroNotSelfTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new HeroNotSelfCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (request.startRequest(requestData, this) && requestData.CasterHex != null && requestData.TargetHex != null && requestData.Target != null)
            {
                //Меняем местами героев
                requestData.CasterHex.SetHero(requestData.Target);
                requestData.TargetHex.SetHero(requestData.Caster);

                if (requestData.TargetHex.OBSTACLE != null && requestData.TargetHex.OBSTACLE is FillableObstacle)
                    (requestData.TargetHex.OBSTACLE as FillableObstacle).ApplyEffect(requestData.Caster, requestData.TargetHex);

                if (requestData.CasterHex.OBSTACLE != null && requestData.CasterHex.OBSTACLE is FillableObstacle)
                    (requestData.CasterHex.OBSTACLE as FillableObstacle).ApplyEffect(requestData.Target, requestData.CasterHex);

                if (requestData.Caster.Team != requestData.Target.Team)
                {
                    ArmorDebuff armorDebuff = new ArmorDebuff(requestData.Caster.Id, extraArmor, 2);
                    requestData.Target.AddEffect(armorDebuff);
                }
                else
                {
                    ArmorBuff armorBuff = new ArmorBuff(requestData.Caster.Id, extraArmor, 2);
                    requestData.Target.AddEffect(armorBuff);
                }
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
                range += 1;
                stats.range -= 1;
                coolDown -= 2;
                stats.coolDown -= 2;
                return true;
            }
            return false;
        }
    }
}
