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
            dmg = 140;
            title = $"Выпускает молнию, которая наносит {dmg} магического урона всем врагам в цепи.";
            titleUpg = "+40 к урону. -1 к перезарядке";
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

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (!request.startRequest(requestData, this, gameData))
                return false;

            if (requestData.Caster != null && requestData.Target != null && requestData.TargetHex != null)
            {
                List<Hero> heroes = new List<Hero>([requestData.Target]);
                heroes = AddNearbyHeroes(requestData.TargetHex, heroes, gameData);

                foreach (var hero in heroes)
                {
                    if (requestData.Caster.Team != hero.Team)
                    {
                        AttackService.SetDamage(requestData.Caster, hero, dmg, dmgType, gameData);
                    }
                }

                requestData.Caster.SpendAP(requireAP, gameData);
                coolDownNow = coolDown;
                return true;
            }
            return false;
        }

        private List<Hero> AddNearbyHeroes(Hex targetHex, List<Hero> heroes, GameData gameData)
        {
            foreach (var hex in UtilityService.GetHexesRadius(targetHex, 1, gameData))
            {
                if (hex.ID != targetHex.ID && hex.HERO != null && heroes.FirstOrDefault(x => x.Id == hex.HERO.Id) == null)
                {
                    heroes.Add(hex.HERO);
                    AddNearbyHeroes(hex, heroes, gameData);
                }
            }
            return heroes;
        }

        public override bool UpgradeSkill(GameData gameData)
        {
            if (!upgraded)
            {
                upgraded = true;
                dmg += 40;
                coolDown -= 1;
                stats.coolDown -= 1;
                title = $"Выпускает молнию, которая наносит {dmg} магического урона всем врагам в цепи.";
                return true;
            }
            return false;
        }
    }
}
