using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.FallenKingSkills
{
    public class RoyalSacrificeSkill : Skill
    {
        int sacrificeHP = 100;
        int percentHeal = 200;
        public RoyalSacrificeSkill()
        {
            name = "Royal Sacrifice";
            dmg = 100;
            title = $"Король жертвует собой во благо победы. Потеряйте {sacrificeHP} ХП и восстановите союзнику {percentHeal}% от пожертвованного ХП." +
                $"\nТакже наносит {dmg} маг. урона врагам вокруг.";
            titleUpg = "+70% к восстановлению ХП, +25 к урону";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            range = 2;
            radius = 1;
            nonTarget = false;
            area = Consts.SpellArea.FriendTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new FriendTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.Target != null && requestData.CasterHex != null)
            {
                int sacrificedHP = requestData.Caster.HP > sacrificeHP ? sacrificeHP : requestData.Caster.HP - 1; // Не допускаем, чтобы герой пожертвовал последнее ХП
                requestData.Caster.HP -= sacrificedHP;

                requestData.Target.Heal((int)Convert.ToDouble(sacrificedHP * percentHeal) / 100);

                foreach (var n in UtilityService.GetHexesRadius(requestData.CasterHex, radius))
                {
                    if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                        AttackService.SetDamage(requestData.Caster, n.HERO, dmg, dmgType);
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
                percentHeal += 70;
                dmg += 25;
                title = $"Король жертвует собой во благо победы. Потеряйте {sacrificeHP} ХП и восстановите союзнику {percentHeal}% от пожертвованного ХП." +
                    $"\nТакже наносит {dmg} маг. урона врагам вокруг.";
                return true;
            }
            return false;
        }
    }
}
