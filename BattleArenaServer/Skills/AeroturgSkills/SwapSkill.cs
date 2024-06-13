using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;
using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;

namespace BattleArenaServer.Skills.AeroturgSkills
{
    public class SwapSkill : Skill
    {
        int extraArmor = 3;
        public SwapSkill()
        {
            name = "Swap";
            title = $"Поменяйтесь местами с любым другим героем. Если это был союзник, то он получает 3 брони, если враг то теряет 3 брони.";
            titleUpg = "+1 к дальности, -2 к перезарядке";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            nonTarget = false;
            range = 3;
            area = Consts.SpellArea.HeroNotSelfTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new HeroNotSelfCastRequest();

        public override bool Cast(Hero caster, Hero? target, Hex? targetHex)
        {
            Hex? casterHex = GameData._hexes.FirstOrDefault(x => x.ID == caster.HexId);
            if (request.startRequest(caster, target, targetHex, this) && casterHex != null && targetHex != null && target != null)
            {
                //Меняем местами героев
                casterHex.SetHero(target);
                targetHex.SetHero(caster);
                if (caster.Team != target.Team)
                {
                    ArmorDebuff armorDebuff = new ArmorDebuff(caster.Id, extraArmor, 2);
                    target.EffectList.Add(armorDebuff);
                    armorDebuff.ApplyEffect(target);
                }
                else
                {
                    ArmorBuff armorBuff = new ArmorBuff(caster.Id, extraArmor, 2);
                    target.EffectList.Add(armorBuff);
                    armorBuff.ApplyEffect(target);
                }
                caster.AP -= requireAP;
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
                range += 1;
                stats.range -= 1;
                coolDown -= 2;
                stats.coolDown -= 2;
                return true;
            }
            return false;
        }
    }
}
