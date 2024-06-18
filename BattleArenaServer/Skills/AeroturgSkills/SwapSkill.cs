using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;
using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Services;

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
                if (requestData.Caster.Team != requestData.Target.Team)
                {
                    ArmorDebuff armorDebuff = new ArmorDebuff(requestData.Caster.Id, extraArmor, 2);
                    requestData.Target.AddEffect(armorDebuff);
                    armorDebuff.ApplyEffect(requestData.Target);
                }
                else
                {
                    ArmorBuff armorBuff = new ArmorBuff(requestData.Caster.Id, extraArmor, 2);
                    requestData.Target.AddEffect(armorBuff);
                    armorBuff.ApplyEffect(requestData.Target);
                }
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
