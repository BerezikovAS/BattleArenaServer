using BattleArenaServer.Models;
using BattleArenaServer.Models.Obstacles;
using BattleArenaServer.Services;
using BattleArenaServer.Skills.InvokerSkills.Auras;

namespace BattleArenaServer.Skills.InvokerSkills.Obstacles
{
    public class MagicBundleObstacle : SolidObstacle
    {
        public MagicBundleObstacle(int id, int casterId, int hexId, int hp, string team, int lifeTime, int reduce_resist, int dmg) : base(id, team, casterId, lifeTime)
        {
            Name = "MagicBundle";
            HexId = hexId;
            MaxHP = HP = hp;
            Armor = 99;
            Dmg = dmg;

            MagicBundleAura magicBundleAura = new MagicBundleAura(false, reduce_resist);
            AuraList.Add(magicBundleAura);

            endLifeEffect += EndLifeMagicBundle;
            this.AddEffect -= this.BaseAddEffect;
        }

        public void EndLifeMagicBundle(Hex currentHex)
        {
            if (HP > 0) // Истекло время действия, а не уничтожили
                foreach (var hex in UtilityService.GetHexesRadius(currentHex, 1))
                {
                    if (hex.HERO != null && hex.HERO.Team != this.Team)
                        AttackService.SetDamage(this, hex.HERO, Dmg, Consts.DamageType.Magic);
                }
        }
    }
}
