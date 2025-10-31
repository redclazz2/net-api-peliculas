using Microsoft.EntityFrameworkCore;
using net_api_peliculas;
using net_api_peliculas.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var allowedOrigins = builder.Configuration.GetValue<string>("AllowedOrigins")!.Split(",");

builder.Services.AddTransient<IAlmacenadorArchivos, AlmacenadorLocal>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(opcionesCORS =>
    {
        opcionesCORS.WithOrigins(allowedOrigins).AllowAnyMethod().AllowAnyHeader().WithExposedHeaders(
            "cantidad-total-registros"
        );
    });
});

builder.Services.AddOutputCache(
    opciones =>
    {
        opciones.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(15);
    }
);

builder.Services.AddDbContext<ApplicationDbContext>(
    opciones => opciones.UseSqlServer("name=DefaultConnection")
);

builder.Services.AddAutoMapper(cfg => { },typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors();

app.UseOutputCache();

app.UseAuthorization();

app.MapControllers();

app.Run();


//Cadena de conexion a la bd: Server=localhost;Database=master;Trusted_Connection=True;