using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;
using BattleArenaServer.Skills.VampireSkills.Obstacles;

namespace BattleArenaServer.Skills.VampireSkills
{
    public class CoffinSkill : Skill
    {
        int coffinHP = 200;
        public CoffinSkill()
        {
            name = "Coffin";
            title = $"Прячет себя или союзника в гроб, прочностью {coffinHP} ХП. Герой в гробу не может действовать. " +
                $"Если через ход гроб не разрушить, то герой вернется и восстановит {coffinHP} ХП.";
            titleUpg = "+50 к прочности гроба и к лечению";
            coolDown = 5;
            coolDownNow = 0;
            requireAP = 1;
            range = 2;
            nonTarget = false;
            area = Consts.SpellArea.AllyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new AllyTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (requestData.Target != null && requestData.Caster != null && requestData.TargetHex != null)
            {
                if (!request.startRequest(requestData, this))
                    return false;

                //Убираем цель с поля
                requestData.TargetHex.RemoveHero();
                requestData.Target.HexId = -1;

                //Ставим гроб
                int Id = GameData._heroes.Max(x => x.Id) + 1;
                CoffinObstacle coffinObstacle
                    = new CoffinObstacle(Id, requestData.Caster.Id, requestData.TargetHex.ID, coffinHP, requestData.Caster.Team, 1, coffinHP, requestData.Target);
                requestData.TargetHex.SetHero(coffinObstacle);
                GameData._solidObstacles.Add(coffinObstacle);

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
                coffinHP += 50;
                title = $"Прячет себя или союзника в гроб, прочностью {coffinHP} ХП. Герой в гробу не может действовать. " +
                    $"Если через ход гроб не разрушить, то герой вернется и восстановит {coffinHP} ХП.";    
                return true;
            }
            return false;
        }
    }
}
