using BattleArenaServer.Hubs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<GameData>();
builder.Services.AddSingleton<FieldHub>();
builder.Services.AddSingleton<LobbyHub>();
builder.Services.AddTransient<IPick, PickService>();
builder.Services.AddTransient<ITiming, TimingService>();
builder.Services.AddTransient<IShop, ShopService>();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddCors(
    options =>
    {
        options.AddDefaultPolicy(
            builder =>
            {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });
    });
builder.Services.AddSignalR();
//builder.Services.AddAuthentication("Cookies").AddCookie(options => options.LoginPath = "/login");
//builder.Services.AddAuthorization();

var app = builder.Build();

app.UseFileServer();
app.UseDefaultFiles();
//app.UseAuthentication();
//app.UseAuthorization();

app.UseCors(builder => builder.WithOrigins("http://localhost:80")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}");

app.MapHub<FieldHub>("/fieldhub");
app.MapHub<LobbyHub>("/lobbyhub");

app.Run();
