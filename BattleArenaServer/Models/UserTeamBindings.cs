namespace BattleArenaServer.Models
{
    public class UserTeamBindings
    {
        public string RedTeam { get; set; }
        public string BlueTeam { get; set; }
        public string ActiveTeam { get; set; }

        public UserTeamBindings() 
        {
            RedTeam = BlueTeam = ActiveTeam = "";
        }

        public void ClearBindings()
        {
            RedTeam = BlueTeam = ActiveTeam = "";
        }
    }
}
