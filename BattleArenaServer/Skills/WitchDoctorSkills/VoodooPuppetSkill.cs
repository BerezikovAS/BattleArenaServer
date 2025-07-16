using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.WitchDoctorSkills
{
    public class VoodooPuppetSkill : Skill
    {
        public VoodooPuppetSkill()
        {
            name = "Voodoo Puppet";
            dmg = 35;
            title = $"Завораживает врага, отчего тот будет повторять Ваши движения. За каждую пройденную клетку враг получает {dmg} маг. урона.";
            titleUpg = "+5 к урону за гекс. Способность не тратит ОД";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            range = 3;
            nonTarget = false;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (!request.startRequest(requestData, this, gameData))
                return false;

            if (requestData.Caster != null && requestData.Target != null)
            {
                VoodooPuppetDebuff voodooPuppetDebuff = new VoodooPuppetDebuff(requestData.Caster.Id, dmg, 1);
                requestData.Target.AddEffect(voodooPuppetDebuff, gameData);

                requestData.Caster.SpendAP(requireAP, gameData);
                coolDownNow = coolDown;
                return true;
            }

            return false;
        }

        public override bool UpgradeSkill(GameData gameData)
        {
            if (!upgraded)
            {
                upgraded = true;
                dmg += 5;
                requireAP = 0;
                stats.requireAP = 0;
                return true;
            }
            return false;
        }
    }
}
