using BattleArenaServer.Effects;
using BattleArenaServer.Effects.Unique;
using BattleArenaServer.Models;
using BattleArenaServer.Models.Obstacles;
using BattleArenaServer.Services;

namespace BattleArenaServer.Skills.WitchDoctorSkills.Obstacles
{
    public class WitchTotemObstacle : SolidObstacle
    {
        private int _radius = 1;
        private int _baseDmg = 0;
        private int _chargeDmg = 0;
        public WitchTotemObstacle(int id, int casterId, int hexId, int hp, string team, int lifeTime, int radius, int baseDmg, int chargeDmg)
            : base(id, team, casterId, lifeTime)
        {
            Name = "WitchTotem";
            HexId = hexId;
            MaxHP = HP = hp;
            endLifeEffect += EndLifeTotem;
            _radius = radius;
            _baseDmg = baseDmg;
            _chargeDmg = chargeDmg;

            Armor = 3;
            Resist = 3;

            this.AddEffect = AddEffectTotem;
        }

        private void AddEffectTotem(Effect effect)
        {
            if (effect is TotemChargeUnique)
                EffectList.Add(effect);
        }

        public void EndLifeTotem(Hex currentHex)
        {
            Effect? charge = this.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.TotemCharge));
            int charges = charge == null ? 0 : charge.value;

            foreach (var hex in UtilityService.GetHexesRadius(currentHex, _radius))
            {
                int dmg = _baseDmg + charges * _chargeDmg;
                if (hex.HERO != null && hex.HERO.Team != this.Team)
                    AttackService.SetDamage(this, hex.HERO, dmg, Consts.DamageType.Pure);
                else if (hex.HERO != null)
                    hex.HERO.Heal(charges * _chargeDmg);
            }
        }
    }
}
