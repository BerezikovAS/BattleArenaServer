using BattleArenaServer;
using BattleArenaServer.Hubs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Services;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddSingleton<IField, FieldService>();
builder.Services.AddSingleton<ITiming, TimingService>();
builder.Services.AddSingleton<FieldHub>();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddSignalR();

var app = builder.Build();

app.UseFileServer();
//app.UseDefaultFiles();

app.UseCors(builder => builder.AllowAnyOrigin());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}");

app.MapHub<FieldHub>("/fieldhub");

app.Run();
