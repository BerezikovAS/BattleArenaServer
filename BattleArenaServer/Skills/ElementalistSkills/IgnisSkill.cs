using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.ElementalistSkills
{
    public class IgnisSkill : Skill
    {
        private int percentLoss = 2;
        private int duration = 2;
        public IgnisSkill()
        {
            name = "Ignis";
            dmg = 100;
            title = $"Огненное дыхание наносит {dmg} магического урона врагам в конусе и поджигает их.\n" +
                $"Горение наносит дополнительный урон в размере {percentLoss}% от максимального запаса ХП при получении любого урона.";
            titleUpg = "Горение действует на 1 ход дольше.";
            coolDown = 4;
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
                        AttackService.SetDamage(requestData.Caster, n.HERO, dmg, dmgType);

                        if (n.HERO != null)
                        {
                            BurnDebuff burnDebuff = new BurnDebuff(requestData.Caster.Id, percentLoss, duration);
                            n.HERO.AddEffect(burnDebuff);
                            burnDebuff.ApplyEffect(n.HERO);
                        }
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
                duration += 1;
                return true;
            }
            return false;
        }
    }
}
