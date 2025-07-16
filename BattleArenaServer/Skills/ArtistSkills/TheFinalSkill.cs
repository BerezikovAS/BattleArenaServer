using BattleArenaServer.Effects;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.ArtistSkills
{
    public class TheFinalSkill : Skill
    {
        int extraDmg = 40;
        public TheFinalSkill()
        {
            name = "The Final";
            dmg = 200;
            title = $"Финал композиции снимает все ноты с цели, чтобы нанести огромный чистый урон. ({dmg} + {extraDmg} * ноты)";
            titleUpg = "-2 к перезарядке, +10 урона за ноту";
            coolDown = 6;
            coolDownNow = 0;
            requireAP = 3;
            nonTarget = false;
            range = 3;
            radius = 0;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Pure;
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (!request.startRequest(requestData, this, gameData))
                return false;

            if (requestData.Caster != null && requestData.Target != null)
            {
                int totalDmg = dmg;
                Effect? note = requestData.Target.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Note));
                if (note != null)
                {
                    totalDmg += note.value * extraDmg;
                    requestData.Target.EffectList.Remove(note);
                }

                AttackService.SetDamage(requestData.Caster, requestData.Target, totalDmg, dmgType, gameData);

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
                extraDmg += 10;
                coolDown -= 2;
                stats.coolDown -= 2;
                title = $"Финал композиции снимает все ноты с цели, чтобы нанести огромный чистый урон. ({dmg} + {extraDmg} * ноты)";
                return true;
            }
            return false;
        }
    }
}
