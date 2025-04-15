using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;
using BattleArenaServer.Effects.Debuffs;

namespace BattleArenaServer.Skills.ChaosSkills
{
    public class ChaosStormSkill : Skill
    {
        public ChaosStormSkill()
        {
            name = "Chaos Storm";
            dmg = 0;
            title = $"Создает хаотичный вихрь, который разбрасывает всех героев в определенной области. Враги получают замедление.";
            titleUpg = "Враги также получают магический урон равный значению атаки одного случайного героя в области.";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            nonTarget = false;
            range = 2;
            radius = 1;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new RangeAoECastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null)
            {
                List<Hero> heroes = new List<Hero>();
                List<Hex> hexes = new List<Hex>();
                foreach (var n in UtilityService.GetHexesRadius(requestData.TargetHex, radius))
                {
                    hexes.Add(n);
                    if (n.HERO != null)
                        heroes.Add(n.HERO);
                    n.RemoveHero();
                }

                Random rnd = new Random();
                Hero heroDmg = heroes[rnd.Next(0, heroes.Count)];
                int dmg = heroDmg.Dmg;

                foreach (Hero hero in heroes)
                {
                    Hex? hex = null;
                    while (hex == null)
                    {
                        hex = hexes.FirstOrDefault(x => x.ID == hexes[rnd.Next(0, hexes.Count)].ID && x.IsFree());
                    }

                    if (hero.Team != requestData.Caster.Team)
                    {
                        SlowDebuff slowDebuff = new SlowDebuff(requestData.Caster.Id, 0, 2);
                        hero.AddEffect(slowDebuff);

                        if (upgraded)
                            AttackService.SetDamage(requestData.Caster, hero, dmg, dmgType);
                    }

                    AttackService.MoveHero(hero, null, hex);
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
                title = $"Создает хаотичный вихрь, который разбрасывает всех героев в определенной области. Враги получают замедление.\n" +
                    $"Враги также получают магический урон равный значению атаки одного случайного героя в области.";
                return true;
            }
            return false;
        }
    }
}
