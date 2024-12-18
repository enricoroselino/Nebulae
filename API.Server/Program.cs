using API.BuildingBlocks;
using API.Server;
using API.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiBuildingBlocks();
builder.Services.AddAssemblyScan(typeof(Program).Assembly);
builder.Services.AddModules();

var app = builder.Build();

app.UseApiBuildingBlocks();
app.UseModules();
app.Run();