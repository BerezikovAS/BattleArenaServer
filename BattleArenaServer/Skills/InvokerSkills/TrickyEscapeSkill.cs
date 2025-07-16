using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;
using BattleArenaServer.Skills.InvokerSkills.Obstacles;

namespace BattleArenaServer.Skills.InvokerSkills
{
    public class TrickyEscapeSkill : Skill
    {
        int obstHP = 80;
        int lifeTime = 2;
        int resistReduce = 2;

        public TrickyEscapeSkill()
        {
            name = "Tricky Escape";
            dmg = 100;
            title = $"Мгновенно телепортирует на свободную клетку, а на прежнем месте появляется сгусток энергии, который снижает сопротивление рядомстоящих врагов" +
                $" на {resistReduce}. А в конце Вашего следующего хода взрывается, нанося {dmg} маг. урона вокруг.";
            titleUpg = "+2 к снижению сопротивления.";
            coolDown = 5;
            coolDownNow = 0;
            requireAP = 1;
            range = 2;
            nonTarget = false;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new HexTargetCastRequest();

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (!request.startRequest(requestData, this, gameData))
                return false;

            if (requestData.Caster != null && requestData.CasterHex != null && requestData.TargetHex != null && requestData.TargetHex.OBSTACLE == null)
            {
                //Телепортируемся
                AttackService.MoveHero(requestData.Caster, requestData.CasterHex, requestData.TargetHex, gameData);

                //Ставим магический сгусток
                int Id = gameData._heroes.Max(x => x.Id) + 1;
                MagicBundleObstacle magicBundleObstacle 
                    = new MagicBundleObstacle(Id, requestData.Caster.Id, requestData.TargetHex.ID, obstHP, requestData.Caster.Team, lifeTime, resistReduce, dmg);
                requestData.CasterHex.SetHero(magicBundleObstacle);
                gameData._solidObstacles.Add(magicBundleObstacle);

                //Обновим ауры
                AttackService.ContinuousAuraAction(gameData);

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
                resistReduce += 2;
                title = $"Мгновенно телепортирует на свободную клетку, а на прежнем месте появляется сгусток энергии, который снижает сопротивление рядомстоящих врагов" +
                $" на {resistReduce}. А в конце Вашего следующего хода взрывается, нанося {dmg} маг. урона вокруг.";
                return true;
            }
            return false;
        }
    }
}
