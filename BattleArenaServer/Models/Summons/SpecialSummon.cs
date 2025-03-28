namespace BattleArenaServer.Models.Summons
{
    public class SpecialSummon : Summon
    {
        public SpecialSummon(int Id, string Team, int casterId, int lifeTime) : base(Id, Team, casterId, lifeTime)
        {
            type = Consts.HeroType.Summon;
            this.casterId = casterId;
            this.lifeTime = lifeTime;
        }


    }
}
