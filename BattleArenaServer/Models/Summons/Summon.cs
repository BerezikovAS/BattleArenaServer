namespace BattleArenaServer.Models.Summons
{
    public class Summon : Hero
    {
        public int casterId { get; set; }
        public int lifeTime { get; set; }

        public Summon(int Id, string Team, int casterId, int lifeTime) : base(Id, Team)
        {
            type = Consts.HeroType.Summon;
            this.casterId = casterId;
            this.lifeTime = lifeTime;
        }
    }
}
