using Microsoft.AspNetCore.Builder;
using MinimalAPIPeliculas.Entidades;
using Microsoft.AspNetCore.Cors;
// using Microsoft.AspNetCore.Builder; // no se ponet porque usa >> < ImplicitUsings > enable </ ImplicitUsings >


var builder = WebApplication.CreateBuilder(args);


// obtener data de appsettings.json
var apellido = builder.Configuration.GetValue<string>("apellido");


//******************** Inicio de �rea de los servicios ***********************
var origenesPermitidos = builder.Configuration.GetValue<string>("origenespermitidos")!;

// habilitar cors
builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(configuracion =>
    {
        // configuracion.WithOrigins().AllowAnyHeader().AllowAnyMethod();
        configuracion.WithOrigins(origenesPermitidos).AllowAnyHeader().AllowAnyMethod();
    });
    opciones.AddPolicy("libre", configuracion =>
    {
        configuracion.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});


// para cacheo de respuestas
builder.Services.AddOutputCache();

// --------middleware para swagger(explora nuestros endpoints)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// -----------------------


//******************* Fin de �rea de los servicios ***********************
var app = builder.Build();
//***********************

// middleware 
app.UseCors();
app.UseOutputCache();

// ----------UseSwagger middleware
if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// http://localhost:5011/swagger/index.html
// -----------------------


//***************Inicio de �rea de los middleware

// app.MapGet("/", () => $"Hello World! {apellido}");
// habilitar solo el cors ("libre") en esta ruta
app.MapGet("/", [EnableCors(policyName: "libre")] () => "�Hola, mundo!");


//http://localhost:5011/generos
app.MapGet("/generos", () =>
{
    var generos = new List<Genero>
    {
        new Genero
        {
            Id = 1,
            Nombre = "Drama"
        },
         new Genero
        {
            Id = 2,
            Nombre = "Acci�n"
        },
          new Genero
        {
            Id = 3,
            Nombre = "Comedia"
        },
    };

    return generos;
})// agregar cacheo a esta ruta
.CacheOutput(c => c.Expire(TimeSpan.FromSeconds(15)));



//*************** Fin de �rea de los middleware




app.Run();
