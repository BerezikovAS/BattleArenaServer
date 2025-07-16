using BattleArenaServer.Models.Lobbies;
using Microsoft.AspNetCore.SignalR;

namespace BattleArenaServer.Hubs
{
    public class LobbyHub : Hub
    {
        private readonly IHubContext<FieldHub> _fieldhub;
        public LobbyHub(IHubContext<FieldHub> fieldhub)
        {
            _fieldhub = fieldhub;
        }

        public override async Task OnConnectedAsync()
        {
            await SendLobbies();
            await base.OnConnectedAsync();
        }

        public async Task SendLobbies()
        {
            await this.Clients.All.SendAsync("RefreshLobbies", LobbyMaster.lobbies);
        }

        public async Task CreateLobby(string name)
        {
            try
            {
                var context = Context.GetHttpContext();
                if (context is not null)
                {
                    if (context.Request.Cookies.ContainsKey("user_id"))
                    {
                        if (context.Request.Cookies.TryGetValue("user_id", out var creatorId) && LobbyMaster.lobbies.FirstOrDefault(x => x.CreatorId == creatorId) == null)
                        {
                            LobbyMaster.lobbies.Add(new Lobby(creatorId, name));
                        }
                    }
                }

                await SendLobbies();
            }
            catch
            {
                Console.WriteLine("Error in CreateLobby()");
            }
        }

        public async Task DeleteLobby(string lobbyId)
        {
            try
            {
                var context = Context.GetHttpContext();
                if (context is not null)
                {
                    if (context.Request.Cookies.ContainsKey("user_id"))
                    {
                        if (context.Request.Cookies.TryGetValue("user_id", out var creatorId) && LobbyMaster.lobbies.FirstOrDefault(x => x.CreatorId == creatorId) != null)
                        {
                            Guid.TryParse(lobbyId, out Guid id);
                            Lobby? lobby = LobbyMaster.lobbies.FirstOrDefault(x => x.CreatorId == creatorId && x.Id == id);
                            
                            if (lobby != null)
                            {
                                await _fieldhub.Clients.Clients(lobby.connectionIds).SendAsync("TransferTo", -1);
                                LobbyMaster.lobbies.Remove(lobby);
                            }
                        }
                    }
                }

                await SendLobbies();
            }
            catch
            {
                Console.WriteLine("Error in CreateLobby()");
            }
        }

        public async Task ConnectToLobby()
        {
            try
            {
                await SendLobbies();
            }
            catch
            {
                Console.WriteLine("Error in CreateLobby()");
            }
        }
    }
}
