using System.Data;
using backend_admon.features.auth.login;
using backend_admon.features.categorias.crear_categoria;
using backend_admon.features.categorias.obtener_categorias;
using backend_admon.features.companias.crear_compania;
using backend_admon.features.condiciones_productos.obtener_condiciones;
using backend_admon.features.expansiones.obtenerExpansiones;
using backend_admon.features.productos.obtener_destacados;
using backend_admon.features.productos.obtener_existentes;
using backend_admon.features.productos.obtener_mas_recientes;
using backend_admon.features.productos.obtener_mas_vendidos;
using backend_admon.features.productos.obtener_productos;
using backend_admon.features.productos.registrar_producto;
using backend_admon.features.rarezas.obtener_rarezas;
using backend_admon.features.tipos_productos.crear_tipo_producto;
using backend_admon.features.tipos_productos.obtener_Tipos;
using backend_admon.features.tipos_productos.obtener_tipos_destacados;
using backend_admon.features.user.registerUser;
using Npgsql;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddTransient<IDbConnection>(sp => new NpgsqlConnection(
    builder.Configuration.GetConnectionString("PostgresConnection")
));

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "Frontend",
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
    );
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();
app.UseCors("Frontend");

app.MapOpenApi();

var api = app.MapGroup("/api");

api.MapRegisterUserEndpoint();
api.MapAuthLoginEndpoint();

api.MapCrearCategoria();
api.MapObtenerCategorias();

api.MapCrearCompania();

api.MapCrearTipoProducto();
api.MapObtenerTipos();
api.MapObtenerTiposDestacados();

api.MapCrearProducto();
api.MapObtenerProductos();
api.MapObtenerProductosDestacados();
api.MapObtenerMasVendidos();
api.MapObtenerMasRecientess();
api.MapObtenerProductosExistencia();

api.MapObtenerExpansiones();

api.MapObtenerRarezas();

api.MapObtenerCondiciones();

app.Run();
