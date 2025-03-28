using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.TinkerSkill
{
    public class SteamStrikeSkill : Skill
    {
        int percentDmg = 20;
        public SteamStrikeSkill()
        {
            name = "Steam Strike";
            dmg = 100;
            title = $"Паровой удар наносит врагам вокруг {dmg} магического урона и ошпаривает их, отчего те по окончании действия эффекта получат еще "
                + $"{percentDmg}% маг. урона от потерянного ХП.";
            titleUpg = $"+15% к урону от потерянного ХП.";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            radius = 1;
            range = 0;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new NonTargerAoECastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null)
            {
                foreach (var hex in UtilityService.GetHexesRadius(requestData.TargetHex, 1))
                {
                    if (hex.HERO != null && hex.HERO.Team != requestData.Caster.Team)
                    {
                        SteamDebuff steamDebuff = new SteamDebuff(requestData.Caster.Id, percentDmg, 2);
                        hex.HERO.AddEffect(steamDebuff);

                        AttackService.SetDamage(requestData.Caster, hex.HERO, dmg, dmgType);
                    }
                }

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
                percentDmg += 15;
                title = $"Паровой удар наносит врагам вокруг {dmg} магического урона и ошпаривает их, отчего те по окончании действия эффекта получат еще "
                + $"{percentDmg}% маг. урона от потерянного ХП.";
                return true;
            }
            return false;
        }
    }
}
