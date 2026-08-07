using Anzuelo.Application.Config;
using Anzuelo.Application.Profiles;
using Anzuelo.Application.Services;
using Anzuelo.Application.Services.Implementations;
using Anzuelo.Application.Services.Interfaces;
using Anzuelo.Infraestructure.Data;
using Anzuelo.Infraestructure.Models;
using Anzuelo.Infraestructure.Repository.Implementations;
using Anzuelo.Infraestructure.Repository.Interfaces;
using Anzuelo.Web.BackgroundServices;
using Anzuelo.Web.Middleware;
using Humanizer.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Configurar los servicios de localización e indicar la carpeta de recursos
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Mapeo de la clase AppConfig para leer appsettings.json
builder.Services.Configure<AppConfig>(builder.Configuration);

// Add services to the container.
builder.Services.AddControllersWithViews(options => {
    options.Filters.Add(
        new ResponseCacheAttribute
        {
            NoStore = true,
            Location = ResponseCacheLocation.None,
        });
})
.AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
.AddDataAnnotationsLocalization();

// Configuración de culturas soportadas
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "es", "en" };
    options.SetDefaultCulture("es")
           .AddSupportedCultures(supportedCultures)
           .AddSupportedUICultures(supportedCultures);
});
//Repository
builder.Services.AddTransient<IRepositoryCombo, RepositoryCombo>();
builder.Services.AddTransient<IRepositoryProducto, RepositoryProducto>();
builder.Services.AddTransient<IRepositoryPreparacion, RepositoryPreparacion>();
builder.Services.AddTransient<IRepositoryMenu, RepositoryMenu>();
builder.Services.AddTransient<IRepositoryUsuario, RepositoryUsuario>();
builder.Services.AddTransient<IRepositoryCategoriaCombo, RepositoryCategoriaCombo>();
builder.Services.AddTransient<IRepositoryCategoriaProducto, RepositoryCategoriaProducto>();
builder.Services.AddTransient<IRepositoryEstadoCombo, RepositoryEstadoCombo>();
builder.Services.AddTransient<IRepositoryEstadoProducto, RepositoryEstadoProducto>();
builder.Services.AddTransient<IRepositoryEstadoMenu, RepositoryEstadoMenu>();
builder.Services.AddTransient<IRepositoryEstacionCocina, RepositoryEstacionCocina>();
builder.Services.AddTransient<IRepositoryIngrediente, RepositoryIngrediente>();
builder.Services.AddTransient<IRepositoryDisponibilidadDia, RepositoryDisponibilidadDia>();
builder.Services.AddTransient<IRepositoryEstadoUsuario, RepositoryEstadoUsuario>();
builder.Services.AddTransient<IRepositoryRol, RepositoryRol>();
builder.Services.AddTransient<IRepositoryDireccion, RepositoryDireccion>();
builder.Services.AddTransient<IRepositoryPedido, RepositoryPedido>();
builder.Services.AddTransient<IRepositoryEstadoPedido, RepositoryEstadoPedido>();
builder.Services.AddTransient<IRepositoryMetodoPago, RepositoryMetodoPago>();
builder.Services.AddTransient<IRepositoryTipoEntrega, RepositoryTipoEntrega>();

//Services
builder.Services.AddTransient<IServiceCombo, ServiceCombo>();
builder.Services.AddTransient<IServiceProducto, ServiceProducto>();
builder.Services.AddTransient<IServicePreparacion, ServicePreparacion>();
builder.Services.AddTransient<IServiceMenu, ServiceMenu>();
builder.Services.AddTransient<IServiceUsuario, ServiceUsuario>();
builder.Services.AddTransient<IServiceCategoriaCombo, ServiceCategoriaCombo>();
builder.Services.AddTransient<IServiceCategoriaProducto, ServiceCategoriaProducto>();
builder.Services.AddTransient<IServiceEstadoCombo, ServiceEstadoCombo>();
builder.Services.AddTransient<IServiceEstadoProducto, ServiceEstadoProducto>();
builder.Services.AddTransient<IServiceEstadoMenu, ServiceEstadoMenu>();
builder.Services.AddTransient<IServiceEstacionCocina, ServiceEstacionCocina>();
builder.Services.AddTransient<IServiceIngrediente, ServiceIngrediente>();
builder.Services.AddTransient<IServiceDisponibilidadDia, ServiceDisponibilidadDia>();
builder.Services.AddTransient<IServiceActualizarMenu, ServiceActualizarMenu>();
builder.Services.AddTransient<IServiceEstadoUsuario, ServiceEstadoUsuario>();
builder.Services.AddTransient<IServiceRol, ServiceRol>();
builder.Services.AddTransient<IServiceDireccion, ServiceDireccion>();
builder.Services.AddTransient<IServicePedido, ServicePedido>();
builder.Services.AddTransient<IServiceEstadoPedido, ServiceEstadoPedido>();
builder.Services.AddTransient<IServiceMetodoPago, ServiceMetodoPago>();
builder.Services.AddTransient<IServiceTipoEntrega, ServiceTipoEntrega>();

builder.Services.AddHostedService<ActualizarMenuCronService>();

//Seguridad
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => {
        options.LoginPath = "/Login/Index";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.AccessDeniedPath = "/Login/Forbidden";
    });

//Configurar Automapper 
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<ComboProfile>();
    config.AddProfile<ProductoProfile>();
    config.AddProfile<PreparacionProfile>();
    config.AddProfile<MenuProfile>();
    config.AddProfile<UsuarioProfile>();
    config.AddProfile<CategoriaComboProfile>();
    config.AddProfile<CategoriaProductoProfile>();
    config.AddProfile<EstadoComboProfile>();
    config.AddProfile<EstadoProductoProfile>();
    config.AddProfile<EstadoMenuProfile>();
    config.AddProfile<EstacionCocinaProfile>();
    config.AddProfile<DisponibilidadDiaProfile>();
    config.AddProfile<RolProfile>();
    config.AddProfile<EstadoUsuarioProfile>();
    config.AddProfile<PedidoProfile>();
    config.AddProfile<DireccionProfile>();
    config.AddProfile<EstadoPedidoProfile>();
    config.AddProfile<TipoEntregaProfile>();
    config.AddProfile<MetodoPagoProfile>();
});

// Configuar Conexión a la Base de Datos SQL 
builder.Services.AddDbContext<AnzueloContext>(options => {
    // it read appsettings.json file 
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDataBase"));

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

//Configuracion Serilog
// Logger. P.E. Verbose = muestra SQl Statement
var logger = new LoggerConfiguration()
                    // Limitar la informacion de depuracion
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                    .Enrich.FromLogContext()
                    // Log LogEventLevel.Verbose muestra mucha informacion, pero no es necesaria solo para el proceso de depuracion
                    .WriteTo.Console(LogEventLevel.Information)
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.File(@"Logs\Info-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug).WriteTo.File(@"Logs\Debug-.log", shared: true, encoding: System.Text.Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.File(@"Logs\Warning-.log", shared: true, encoding: System.Text.Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.File(@"Logs\Error-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal).WriteTo.File(@"Logs\Fatal-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .CreateLogger();

builder.Host.UseSerilog(logger);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    // Error control Middleware
    app.UseMiddleware<ErrorHandlingMiddleware>();
}

//Activar soporte a la solicitud de registro con SERILOG 
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);

app.UseAuthentication();

app.UseAuthorization();

// Activar Antiforgery  
app.UseAntiforgery();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();