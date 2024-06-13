using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.AeroturgSkills
{
    public class ChainLightningSkill : Skill
    {
        int dmg = 150;
        public ChainLightningSkill()
        {
            name = "Chain Lightning";
            title = $"Выпускает молнию, которая наносит {dmg} магического урона всем врагам в цепи.";
            titleUpg = "+50 к урону. -1 к перезарядке";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 3;
            area = Consts.SpellArea.HeroTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new HeroTargetCastRequest();

        public override bool Cast(Hero caster, Hero? target, Hex? targetHex)
        {
            if (request.startRequest(caster, target, targetHex, this))
            {
                if (caster != null && target != null && targetHex != null)
                {
                    List<Hero> heroes = new List<Hero>([target]);
                    heroes = AddNearbyHeroes(targetHex, heroes);

                    foreach (var hero in heroes)
                    {
                        if (caster.Team != hero.Team)
                        {
                            AttackService.SetDamage(caster, hero, dmg, Consts.DamageType.Magic);
                        }
                    }

                    caster.AP -= requireAP;
                    coolDownNow = coolDown;
                    return true;
                }
            }
            return false;
        }

        private List<Hero> AddNearbyHeroes(Hex targetHex, List<Hero> heroes)
        {
            foreach (var hex in UtilityService.GetHexesRadius(targetHex, 1))
            {
                if (hex.ID != targetHex.ID && hex.HERO != null && heroes.FirstOrDefault(x => x.Id == hex.HERO.Id) == null)
                {
                    heroes.Add(hex.HERO);
                    AddNearbyHeroes(hex, heroes);
                }
            }
            return heroes;
        }

        public override bool UpgradeSkill()
        {
            if (!upgraded)
            {
                upgraded = true;
                dmg += 50;
                coolDown -= 1;
                stats.coolDown -= 1;
                return true;
            }
            return false;
        }
    }
}
