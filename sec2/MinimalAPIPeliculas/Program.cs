var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// obtener data de appsettings.json
var apellido = builder.Configuration.GetValue<string>("apellido");

app.MapGet("/", () => $"Hello World! {apellido}");

app.Run();
