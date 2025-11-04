using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using net_api_peliculas;
using net_api_peliculas.Servicios;
using net_api_peliculas.Utilidades;
using NetTopologySuite;
using NetTopologySuite.Geometries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddIdentityCore<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<IdentityUser>>();
builder.Services.AddScoped<SignInManager<IdentityUser>>();

builder.Services.AddAuthentication().AddJwtBearer(opciones =>
{
    opciones.MapInboundClaims = false;
    opciones.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                builder.Configuration["llavejwt"]!
            )
        ),
        ClockSkew = TimeSpan.Zero
    };
});

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

builder.Services.AddSingleton<GeometryFactory>(
    NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326)
);

builder.Services.AddOutputCache(
    opciones =>
    {
        opciones.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(15);
    }
);

builder.Services.AddDbContext<ApplicationDbContext>(
    opciones => opciones.UseSqlServer("name=DefaultConnection", sqlServer => 
        sqlServer.UseNetTopologySuite())
);

//builder.Services.AddAutoMapper(cfg => { },typeof(Program));

builder.Services.AddSingleton(proveedor => new MapperConfiguration(configuration =>{
    var geometryFactory = proveedor.GetRequiredService<GeometryFactory>();
    configuration.AddProfile(new AutoMapperProfiles(geometryFactory));
}).CreateMapper());

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