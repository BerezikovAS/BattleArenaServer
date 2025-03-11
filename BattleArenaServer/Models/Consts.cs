namespace BattleArenaServer.Models
{
    public static class Consts
    {
        public static List<Hex> directions = new List<Hex>() {
            new Hex(new int[3]{ 1, -1, 0 }), //право-верх   0
            new Hex(new int[3]{ 1, 0, -1 }), //право        1
            new Hex(new int[3]{ 0, 1, -1 }), //право-низ    2
            new Hex(new int[3]{ -1, 1, 0 }), //лево-низ     3
            new Hex(new int[3]{ -1, 0, 1 }), //лево         4
            new Hex(new int[3]{ 0, -1, 1 })  //лево-верх    5
        };

        public enum SpellArea
        {
            NonTarget,
            AllyTarget,
            EnemyTarget,
            FriendTarget,
            Radius,
            Line,
            Conus,
            HeroTarget,
            HeroNotSelfTarget,
            SmallConus
        }

        public enum DamageType
        {
            Physical,
            Magic,
            Pure
        }

        public enum AuraType
        {
            Continuous,
            EndTurn,
            StartTurn
        }

        public enum EffectType
        {
            Instant,
            EndTurn,
            StartTurn
        }

        public enum HeroType
        {
            Hero,
            Obstacle
        }

        public enum SkillType
        {
            Active,
            Passive
        }

        public enum StatusEffect
        {
            Buff,
            Debuff,
            Unique
        }
    }
}
