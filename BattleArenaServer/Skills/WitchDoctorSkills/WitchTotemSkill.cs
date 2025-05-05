using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;
using BattleArenaServer.Skills.WitchDoctorSkills.Auras;
using BattleArenaServer.Skills.WitchDoctorSkills.Obstacles;

namespace BattleArenaServer.Skills.WitchDoctorSkills
{
    public class WitchTotemSkill : Skill
    {
        int totemHP = 150;
        int lifeTime = 3;
        int baseDmg = 15;
        int chargeDmg = 10;

        public WitchTotemSkill()
        {
            name = "WitchTotem";
            dmg = 20;
            title = $"Размещает древний тотем, который в конце Вашего хода наносит каждому врагу {dmg} чистого урона и увеличивает свой заряд на 1." +
                $"\nЕсли тотем уничтожен или срок жизни подошел к концу, то врагам в радиусе наносится урон, а союзники восстанавливают ХП в зависимости от зарядов." +
                $"\nУрон = {baseDmg} + 'Заряды' * {chargeDmg}, Лечение = 'Заряды' * {chargeDmg}";
            titleUpg = "+5 к урону за заряд. +50 к ХП.";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            range = 2;
            nonTarget = false;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Pure;
        }

        public new ISkillCastRequest request => new HexTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null && requestData.TargetHex.OBSTACLE == null)
            {
                //Ставим тотем
                int Id = GameData._heroes.Max(x => x.Id) + 1;
                WitchTotemObstacle witchTotemObstacle = 
                    new WitchTotemObstacle(Id, requestData.Caster.Id, requestData.TargetHex.ID, totemHP, requestData.Caster.Team, lifeTime, 2, baseDmg, chargeDmg);
                requestData.TargetHex.SetHero(witchTotemObstacle);
                GameData._solidObstacles.Add(witchTotemObstacle);

                // Добавляем ауру нашему тотему
                WitchTotemAura witchTotemAura = new WitchTotemAura(dmg);
                witchTotemObstacle.AuraList.Add(witchTotemAura);

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
                chargeDmg += 5;
                totemHP += 50;
                title = $"Размещает древний тотем, который в конце Вашего хода наносит каждому врагу {dmg} чистого урона и увеличивает свой заряд на 1." +
                $"\nЕсли тотем уничтожен или срок жизни подошел к концу, то врагам в радиусе наносится урон, а союзники восстанавливают ХП в зависимости от зарядов." +
                $"\nУрон = {baseDmg} + 'Заряды' * {chargeDmg}, Лечение = 'Заряды' * {chargeDmg}";
                return true;
            }
            return false;
        }
    }
}
