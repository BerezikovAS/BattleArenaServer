using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.PlagueDoctorSkills
{
    public class VaccineSkill : Skill
    {
        int heal = 20;
        int extraHeal = 8;
        public VaccineSkill()
        {
            name = "Vaccine";
            title = $"Вводит долгодействующую вакцину в себя или союзника. В начале своего хода пациент восстанавливает ХП в зависимости от " +
                $"количества негативных эффектов и снимает 1 случайный с себя.";
            titleUpg = "+10 к лечению, -1 к перезарядке";
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
            if (requestData.Target != null && requestData.Caster != null)
            {
                if (!request.startRequest(requestData, this))
                    return false;

                VaccineBuff vaccineBuff = new VaccineBuff(requestData.Caster.Id, heal, 3, extraHeal);
                requestData.Target.AddEffect(vaccineBuff);

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
                heal += 10;
                coolDown -= 1;
                stats.coolDown -= 1;
                return true;
            }
            return false;
        }
    }
}
