using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.ElementalistSkills
{
    public class TerraSkill : Skill
    {
        private int shieldDurability = 130;
        public TerraSkill()
        {
            name = "Terra";
            title = $"Создает каменный щит вокруг себя или союзника, который поглощает {shieldDurability} физического урона.";
            titleUpg = "+45 к прочности щита";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            range = 3;
            nonTarget = false;
            area = Consts.SpellArea.AllyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new AllyTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (requestData.Target != null && requestData.Caster != null)
            {
                if (!request.startRequest(requestData, this))
                    return false;

                PhysShieldBuff physShieldBuff = new PhysShieldBuff(requestData.Caster.Id, shieldDurability, 3);
                requestData.Target.AddEffect(physShieldBuff);

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
                shieldDurability += 45;
                title = $"Создает каменный щит вокруг себя или союзника, который поглощает {shieldDurability} физического урона.";
                return true;
            }
            return false;
        }
    }
}
