﻿using BattleArenaServer.Effects;
using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.WitchDoctorSkills
{
    public class PurificationSkill : Skill
    {
        public PurificationSkill()
        {
            name = "Purification";
            title = $"Очищающий свет снимает негативные эффекты с союзников. За каждый снятый эффект герой получает +1 брони и +1 сопротивления.";
            titleUpg = "-3 к перезарядке";
            coolDown = 5;
            coolDownNow = 0;
            requireAP = 1;
            radius = 1;
            range = 2;
            nonTarget = false;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new RangeAoECastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null)
            {
                foreach (var hex in UtilityService.GetHexesRadius(requestData.TargetHex, radius))
                {
                    if (hex.HERO != null && hex.HERO.Team == requestData.Caster.Team)
                    {
                        List<Effect> debuffs = new List<Effect>();
                        foreach (var debuff in hex.HERO.EffectList.Where(x => x.type == Consts.StatusEffect.Debuff))
                        {
                            debuffs.Add(debuff);
                            debuff.RemoveEffect(hex.HERO);
                        }

                        int debuffCount = debuffs.Count;
                        if (debuffCount > 0)
                        {
                            ArmorBuff armorBuff = new ArmorBuff(requestData.Caster.Id, debuffCount, 2);
                            hex.HERO.AddEffect(armorBuff);
                            armorBuff.ApplyEffect(hex.HERO);

                            ResistBuff resistBuff = new ResistBuff(requestData.Caster.Id, debuffCount, 2);
                            hex.HERO.AddEffect(resistBuff);
                            resistBuff.ApplyEffect(hex.HERO);
                        }

                        foreach (var effect in debuffs)
                        {
                            hex.HERO.EffectList.Remove(effect);
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
                coolDown -= 3;
                stats.coolDown -= 3;
                title = $"Очищающий свет снимает негативные эффекты с союзников. За каждый снятый эффект герой получает +1 брони и +1 сопротивления.";
                return true;
            }
            return false;
        }
    }
}
