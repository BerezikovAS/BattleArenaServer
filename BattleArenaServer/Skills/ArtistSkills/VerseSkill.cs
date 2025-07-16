using BattleArenaServer.Effects;
using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.ArtistSkills
{
    public class VerseSkill : Skill
    {
        int extraDmg = 10;
        public VerseSkill()
        {
            name = "Verse";
            dmg = 80;
            title = $"Музыкальный куплет накладывает 1 ноту на врагов в области и наносит им {dmg} маг. урона. Урон увеличивается на {extraDmg} за каждую ноту.";
            titleUpg = "+10 к урону за ноту.";
            coolDown = 1;
            coolDownNow = 0;
            requireAP = 2;
            range = 3;
            radius = 3;
            nonTarget = false;
            area = Consts.SpellArea.WideLine;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new LineCastRequest();

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (!request.startRequest(requestData, this, gameData))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null && requestData.CasterHex != null)
            {
                foreach (var n in UtilityService.GetHexesWideLine(requestData.CasterHex, requestData.TargetHex, radius, gameData))
                {
                    if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                    {
                        Effect? noteDebuff = n.HERO.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Note));
                        if (noteDebuff != null)
                            noteDebuff.ApplyEffect(n.HERO, gameData); // В методе есть инкремент количества нот
                        else
                        {
                            noteDebuff = new NoteDebuff(requestData.Caster.Id, 0, 3, requestData.Caster.Name);
                            n.HERO.AddEffect(noteDebuff, gameData);
                        }

                        // У врага мог быть иммун к дебафам, поэтому ищем ноты на герое заново
                        Effect? note = n.HERO.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Note));
                        int totalDmg = dmg + extraDmg * (note == null ? 0 : note.value);

                        AttackService.SetDamage(requestData.Caster, n.HERO, totalDmg, dmgType, gameData);
                    }
                }
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
                title = $"Музыкальный куплет наносит {dmg} маг. урона и накладывает 1 ноту на врагов в области. Урон увеличивается на {extraDmg} за каждую ноту.";
                return true;
            }
            return false;
        }
    }
}
