using Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<MessageService>();

var app = builder.Build();

app.MapControllers();

app.Run();
