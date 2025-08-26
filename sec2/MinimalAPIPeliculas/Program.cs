using Microsoft.AspNetCore.Builder;
using MinimalAPIPeliculas.Entidades;

// using Microsoft.AspNetCore.Builder; // no se ponet porque usa >> < ImplicitUsings > enable </ ImplicitUsings >


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// obtener data de appsettings.json
var apellido = builder.Configuration.GetValue<string>("apellido");


//******************** Inicio de �rea de los servicios ***********************


//******************* Fin de �rea de los servicios ***********************


//***************Inicio de �rea de los middleware

app.MapGet("/", () => $"Hello World! {apellido}");

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
});


//*************** Fin de �rea de los middleware




app.Run();
