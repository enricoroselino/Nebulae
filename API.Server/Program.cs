using API.BuildingBlocks;
using API.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiBuildingBlocks();
builder.Services.AddModules();

var app = builder.Build();

app.UseApiBuildingBlocks();
app.UseModules();
app.Run();