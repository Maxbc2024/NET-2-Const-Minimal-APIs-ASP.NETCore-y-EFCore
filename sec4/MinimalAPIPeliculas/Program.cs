
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using MinimalAPIPeliculas.Entidades;
using Microsoft.AspNetCore.Cors;
using MinimalAPIPeliculas;
using MinimalAPIPeliculas.Repositorios;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIPeliculas.Endpoints;
// using Microsoft.AspNetCore.Builder; // no se ponet porque usa >> < ImplicitUsings > enable </ ImplicitUsings >


var builder = WebApplication.CreateBuilder(args);


// obtener data de appsettings.json
var apellido = builder.Configuration.GetValue<string>("apellido");


//******************** Inicio de �rea de los servicios ***********************
var origenesPermitidos = builder.Configuration.GetValue<string>("origenespermitidos")!;



//*************************BD SQLLITE*********************************************
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//**********************************************************************



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
// ----------------------- MIS SERVICIOS

builder.Services.AddScoped<IRepositorioGeneros, RepositorioGeneros>();

// automapper
builder.Services.AddAutoMapper(typeof(Program));

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




// //http://localhost:5011/generos
// app.MapGet("/generos", async (IRepositorioGeneros repositorio) =>
// {
//     return await repositorio.ObtenerTodos();
// })// agregar cacheo a esta ruta
// .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60))
// .Tag("generos-get")); //Tag > es un nombre del cache > para llamarlo de otro lado

// app.MapPost("/generos", async (Genero genero, IRepositorioGeneros repositorio,IOutputCacheStore outputCacheStore) =>
// {
//     var id = await repositorio.Crear(genero);
//     await outputCacheStore.EvictByTagAsync("generos-get", default);// limpia el cache "generos-get"
//     return Results.Created($"/generos/{id}", genero);
// });



app.MapGroup("/generos").MapGeneros();

//*************** Fin de �rea de los middleware




app.Run();
