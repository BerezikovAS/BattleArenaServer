using BattleArenaServer;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IField, FieldService>();
builder.Services.AddSingleton<ITiming, TimingService>();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddCors();

var app = builder.Build();

app.UseCors(builder => builder.AllowAnyOrigin());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}");

app.Run();
