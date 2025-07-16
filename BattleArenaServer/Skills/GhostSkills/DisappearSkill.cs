using BattleArenaServer.Effects.Unique;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.GhostSkills
{
    public class DisappearSkill : Skill
    {
        public DisappearSkill()
        {
            name = "Disappear";
            dmg = 100;
            title = $"Выберите область. Враги в ней получат {dmg} маг. урона, а Вы исчезнете и появитесь в этой области в начале след. хода " +
                $"и нанесете еще {dmg} урона вокруг себя.";
            titleUpg = "+30 урону";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            nonTarget = false;
            range = 2;
            radius = 1;
            area = Consts.SpellArea.Radius;
            dmgType = Consts.DamageType.Magic;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new RangeAoECastRequest();

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (!request.startRequest(requestData, this, gameData))
                return false;

            if (requestData.Caster != null && requestData.CasterHex != null && requestData.TargetHex != null)
            {
                List<Hex> list = UtilityService.GetHexesRadius(requestData.TargetHex, radius, gameData);
                foreach (var n in list)
                {
                    if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                    {
                        AttackService.SetDamage(requestData.Caster, n.HERO, dmg, Consts.DamageType.Magic, gameData);
                    }
                }

                DisappearUnique disappearUnique = new DisappearUnique(requestData.Caster.Id, dmg, 2, requestData.CasterHex, requestData.TargetHex, list);
                requestData.Caster.AddEffect(disappearUnique, gameData);
                disappearUnique.ApplyEffect(requestData.Caster, gameData);

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
                dmg += 30;
                title = $"Выберите область. Враги в ней получат {dmg} маг. урона, а Вы исчезнете и появитесь в этой области в начале след. хода " +
                    $"и нанесете еще {dmg} урона вокруг себя.";
                return true;
            }
            return false;
        }
    }
}
