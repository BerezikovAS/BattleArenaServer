using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.GeomantSkills
{
    public class StoneBloodSkill : Skill
    {
        int extraRegen = 8;
        public StoneBloodSkill()
        {
            name = "Stone Blood";
            title = $"Вбирает в себя мощь земли, получая регенерацию здоровья в зависимости от свобоных гексов вокруг себя. Столоктиты считаются свободными и дают двойной бонус.\n" +
                $"(+{extraRegen} к восстановлению ХП за гекс, регенерация длится 3 хода)";
            titleUpg = "+6 к восстановлению ХП за гекс";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            radius = 1;
            nonTarget = false;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new NonTargerAoECastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.CasterHex != null)
            {
                int freeHexCount = 0;

                foreach (var hex in UtilityService.GetHexesRadius(requestData.CasterHex, radius))
                {
                    if (hex.IsFree())
                        freeHexCount += 1;
                    else if (hex.HERO?.Name == "Stalaktite")
                        freeHexCount += 2;
                }

                RegenerationBuff regenerationBuff = new RegenerationBuff(requestData.Caster.Id, freeHexCount * extraRegen, 3);
                requestData.Caster.AddEffect(regenerationBuff);

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
                extraRegen += 6;
                title = $"Вбирает в себя мощь земли, получая регенерацию здоровья в зависимости от свобоных гексов вокруг себя. Столоктиты считаются свободными и дают двойной бонус.\n" +
                    $"(+{extraRegen} к восстановлению ХП за гекс, регенерация длится 3 хода)";
                return true;
            }
            return false;
        }
    }
}
