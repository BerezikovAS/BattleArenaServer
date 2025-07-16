using BattleArenaServer.Effects;
using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.ArtistSkills
{
    public class ChorusSkill : Skill
    {
        int durability = 60;
        int extraDurability = 5;
        public ChorusSkill()
        {
            name = "Chorus";
            title = $"Мелодичный припев даёт союзникам 1 ОД и дарует щиты на {durability} прочности. Щиты имеют +{extraDurability} прочности за каждую ноту на враге в радиусе заклинания.";
            titleUpg = "+1 к радиусу, +15 к прочности щита";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            radius = 2;
            range = 0;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new NonTargerAoECastRequest();

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (request.startRequest(requestData, this, gameData))
            {
                if (requestData.Caster != null && requestData.TargetHex != null)
                {
                    int noteCount = 0;
                    foreach (var n in UtilityService.GetHexesRadius(requestData.TargetHex, radius, gameData))
                    {
                        if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                        {
                            Effect? note = n.HERO.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Note));
                            if (note != null)
                                noteCount += note.value;
                        }
                    }

                    foreach (var n in UtilityService.GetHexesRadius(requestData.TargetHex, radius, gameData))
                    {
                        if (n.HERO != null && n.HERO.Team == requestData.Caster.Team)
                        {
                            n.HERO.AP += 1;
                            DmgShieldBuff dmgShieldBuff = new DmgShieldBuff(requestData.Caster.Id, durability + extraDurability * noteCount, 2);
                            n.HERO.AddEffect(dmgShieldBuff, gameData);
                        }
                    }
                    requestData.Caster.SpendAP(requireAP, gameData);
                    coolDownNow = coolDown;
                    return true;
                }
            }
            return false;
        }

        public override bool UpgradeSkill(GameData gameData)
        {
            if (!upgraded)
            {
                upgraded = true;
                radius += 1;
                stats.radius += 1;
                durability += 15;
                return true;
            }
            return false;
        }
    }
}
