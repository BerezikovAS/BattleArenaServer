using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.AeroturgSkills
{
    public class ChainLightningSkill : Skill
    {
        public ChainLightningSkill()
        {
            name = "Chain Lightning";
            dmg = 150;
            title = $"Выпускает молнию, которая наносит {dmg} магического урона всем врагам в цепи.";
            titleUpg = "+50 к урону. -1 к перезарядке";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 3;
            area = Consts.SpellArea.HeroTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new HeroTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.Target != null && requestData.TargetHex != null)
            {
                List<Hero> heroes = new List<Hero>([requestData.Target]);
                heroes = AddNearbyHeroes(requestData.TargetHex, heroes);

                foreach (var hero in heroes)
                {
                    if (requestData.Caster.Team != hero.Team)
                    {
                        AttackService.SetDamage(requestData.Caster, hero, dmg, dmgType);
                    }
                }

                requestData.Caster.AP -= requireAP;
                coolDownNow = coolDown;
                return true;
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
                title = $"Выпускает молнию, которая наносит {dmg} магического урона всем врагам в цепи.";
                return true;
            }
            return false;
        }
    }
}
