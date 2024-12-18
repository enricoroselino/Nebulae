using API.BuildingBlocks;
using API.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiBuildingBlocks();
builder.Services.AddAssemblyScan(typeof(Program).Assembly);

var app = builder.Build();

app.UseApiBuildingBlocks();
app.Run();