using System.Data;
using System.Text.Json.Serialization.Metadata;
using backend_admon.features.user.registerUser;
using Npgsql;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddTransient<IDbConnection>(sp =>
    new NpgsqlConnection(
        builder.Configuration.GetConnectionString("PostgresConnection")
    )
);

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly)
);

var app = builder.Build();

app.MapOpenApi();

app.MapRegisterUserEndpoint();

app.Run();