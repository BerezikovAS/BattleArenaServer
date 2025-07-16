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
            titleUpg = "+1 к радиусу поражения";
            coolDown = 5;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 2;
            radius = 2;
            area = Consts.SpellArea.Conus;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new LineCastRequest();

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (!request.startRequest(requestData, this, gameData))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null && requestData.CasterHex != null)
            {
                foreach (var n in UtilityService.GetHexesCone(requestData.CasterHex, requestData.TargetHex, radius, gameData))
                {
                    if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                    {
                        DizzinessDebuff dizzinessDebuff = new DizzinessDebuff(requestData.Caster.Id, 0, 2);
                        n.HERO.AddEffect(dizzinessDebuff, gameData);

                        AttackService.SetDamage(requestData.Caster, n.HERO, dmg, dmgType, gameData);
                    }
                }
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
                range += 1;
                radius += 1;
                stats.range += 1;
                stats.radius += 1;
                title = $"Волшебная руна издаёт оглушающий грохот. Враги в области получают {dmg} маг. урона и дезориентируются, отчего их передвижение становится неконтролируемым.";
                return true;
            }
            return false;
        }
    }
}
