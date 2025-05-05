using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.DwarfSkills
{
    public class ThunderclapSkill : Skill
    {
        public ThunderclapSkill()
        {
            name = "Thunderclap";
            dmg = 135;
            title = $"Волшебная руна издаёт оглушающий грохот. Враги в области получают {dmg} маг. урона и дезориентируются, отчего их передвижение становится неконтролируемым.";
            titleUpg = "+30 к урону";
            coolDown = 5;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 3;
            radius = 3;
            area = Consts.SpellArea.Conus;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new LineCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null && requestData.CasterHex != null)
            {
                foreach (var n in UtilityService.GetHexesCone(requestData.CasterHex, requestData.TargetHex, radius))
                {
                    if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                    {
                        DizzinessDebuff dizzinessDebuff = new DizzinessDebuff(requestData.Caster.Id, 0, 2);
                        n.HERO.AddEffect(dizzinessDebuff);

                        AttackService.SetDamage(requestData.Caster, n.HERO, dmg, dmgType);
                    }
                }
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
                dmg += 30;
                title = $"Волшебная руна издаёт оглушающий грохот. Враги в области получают {dmg} маг. урона и дезориентируются, отчего их передвижение становится неконтролируемым.";
                return true;
            }
            return false;
        }
    }
}
