using BattleArenaServer.Effects;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills._CommonSkills
{
    public class MoveSkill : Skill
    {
        public MoveSkill()
        {
            name = "Move";
            title = $"Перемещение на соседний гекс.";
            titleUpg = "";
            coolDown = 0;
            coolDownNow = 0;
            requireAP = 1;
            nonTarget = false;
            range = 1;
            area = Consts.SpellArea.NonTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new MoveCastRequest();

        public override bool Cast(RequestData requestData)
        {
            //Проверка на Fear (Можно перемещаться только в направлении ОТ заклинателя. Т.е. дистанция должна увеличиваться)
            Effect? fear = requestData.Caster?.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Fear));
            if (fear != null)
            {
                Hex? fearSource = GameData._hexes.FirstOrDefault(x => x.HERO != null && x.HERO.Id == fear.idCaster);
                if (fearSource != null && fearSource.Distance(requestData.TargetHex) <= fearSource.Distance(requestData.CasterHex))
                    return false;
            }

            //Проверка на Slow (Перемещение требует 2 ОД)
            Effect? slow = requestData.Caster?.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Slow));
            if (slow != null)
                requireAP = 2;

            //Проверка на Haste (Перемещение не требует ОД, но сам бафф действует на одно перемещение)
            Effect? haste = requestData.Caster?.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Haste));
            if (haste != null)
                requireAP = 0;

            //Проверка на Dizziness (Перемещение совершается на случайную клетку по направлению движения)
            Effect? dizziness = requestData.Caster?.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Dizziness));
            if (dizziness != null)
            {
                List<Hex> availableHexes = GameData._hexes.FindAll(x => x.IsFree() && x.Distance(requestData.TargetHex) == 1 && x.Distance(requestData.CasterHex) == 1);
                availableHexes.Add(requestData.TargetHex);

                Random rnd = new Random();
                Hex targetHexNew = availableHexes[rnd.Next(availableHexes.Count())];
                requestData.TargetHex = targetHexNew;
            }

            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.CasterHex != null && requestData.TargetHex != null)
            {
                if (haste == null)
                    requestData.Caster.SpendAP(requireAP);
                else
                    requestData.Caster.EffectList.Remove(haste);

                requireAP = 1;
                AttackService.MoveHero(requestData.Caster, requestData.CasterHex, requestData.TargetHex);
                return true;
            }
            requireAP = 1;
            return false;
        }

        public override bool UpgradeSkill()
        {
            return false;
        }
    }
}
