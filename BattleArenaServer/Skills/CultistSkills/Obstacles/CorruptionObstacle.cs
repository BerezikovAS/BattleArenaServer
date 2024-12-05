using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Models.Obstacles;
using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Skills.CultistSkills.Obstacles
{
    public class CorruptionObstacle : FillableObstacle
    {
        int Dmg;
        public CorruptionObstacle(int casterId, int hexId, int lifeTime, string team, int dmg)
        {
            Name = "Corruption";
            CasterId = casterId;
            HexId = hexId;
            LifeTime = lifeTime;
            Team = team;
            Dmg = dmg;
        }
        public override void ApplyEffect(Hero hero, Hex hex)
        {
            if (hero.Team != Team)
            {
                //Наносим мгновенный дамаг
                Hero? attacker = GameData._heroes.FirstOrDefault(x => x.Id == CasterId);
                AttackService.SetDamage(attacker, hero, Dmg, Consts.DamageType.Pure);

                //Лечимся
                if (attacker != null)
                    attacker.Heal(Dmg);
            }
        }
    }
}
