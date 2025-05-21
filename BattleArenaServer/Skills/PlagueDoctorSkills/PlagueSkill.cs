using BattleArenaServer.Effects;
using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.PlagueDoctorSkills
{
    public class PlagueSkill : Skill
    {
        int resistReduction = 2;
        public PlagueSkill()
        {
            name = "Plague";
            dmg = 160;
            title = $"Заражает выбранного врага чумой и наносит тому {dmg} маг. урона. " +
                $"Зараженный враг теряет {resistReduction} сопротивления и заразит других врагов вокруг себя в конце своего хода.";
            titleUpg = "+25 к урону, +1 к снижению сопротивления";
            coolDown = 5;
            coolDownNow = 0;
            requireAP = 2;
            range = 2;
            nonTarget = false;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.Target != null)
            {
                Effect? effect = requestData.Target.EffectList.FirstOrDefault(x => x.Name == "Plague");
                if (effect == null)
                {
                    PlagueDebuff plagueDebuff = new PlagueDebuff(requestData.Caster.Id, resistReduction, 2, GameData.turn - 1);
                    requestData.Target.AddEffect(plagueDebuff);
                }

                AttackService.SetDamage(requestData.Caster, requestData.Target, dmg, dmgType);

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
                dmg += 25;
                resistReduction += 1;
                title = $"Заражает выбранного врага чумой и наносит тому {dmg} маг. урона. " +
                    $"Зараженный враг теряет {resistReduction} сопротивления и заразит других врагов вокруг себя в конце своего хода.";
                return true;
            }
            return false;
        }
    }
}
