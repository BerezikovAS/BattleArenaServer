namespace BattleArenaServer.Models.Obstacles
{
    public class SolidObstacle : Hero
    {
        public int casterId { get; set; }
        public int lifeTime { get; set; }

        public SolidObstacle(int Id, string Team, int casterId, int lifeTime) : base(Id, Team)
        {
            type = Consts.HeroType.Obstacle;
            this.casterId = casterId;
            this.lifeTime = lifeTime;

            VP = 0;
            GoldReward = 1;
            IsMainHero = false;
        }

        public delegate void EndLifeEffect(Hex currentHex);
        public EndLifeEffect endLifeEffect = delegate { };
    }
}
