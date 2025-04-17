using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ServerVanilla.Persistence;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("+=====================================+");
Log.Information("+              EXVS2-POC              +");
Log.Information("+                                     +");
Log.Information("+ Disclaimer:                         +");
Log.Information("+ FREE SOFTWARE, BEWARE OF SCAMMERS!  +");
Log.Information("+ IF YOU PAID, YOU ARE BEING SCAMMED! +");
Log.Information("+=====================================+");

Log.Information("Server starting...");

try
{
    var builder = WebApplication.CreateBuilder(args);
    
    const string configurationsDirectory = "Configurations";
    builder.Configuration.AddJsonFile($"{configurationsDirectory}/kestrel.json", optional: false)
        .AddJsonFile($"{configurationsDirectory}/log.json", optional: false)
        .AddJsonFile($"{configurationsDirectory}/server.json", optional: false);

    // builder.Services.AddOptions<CardServerConfig>()
    //     .Bind(builder.Configuration.GetSection(CardServerConfig.CARD_SERVER_SECTION))
    //     .ValidateMiniValidation()
    //     .ValidateOnStart();

    // Add services to the container.
    builder.Host.UseSerilog((context, configuration) =>
    {
        configuration.WriteTo.Console().ReadFrom.Configuration(context.Configuration);
    });

    builder.Services.AddMediatR(cfg => {
        cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    });
    builder.Services.AddControllers().AddProtoBufNet();

    builder.Services.AddDbContext<ServerDbContext>(options => 
        options.UseSqlite(
            new SqliteConnectionStringBuilder() { DataSource = "ServerVS2.db" }.ConnectionString,
            o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
        )
    );
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    
    builder.Services.AddMemoryCache();
    
    var app = builder.Build();
    
    // var config = app.Services.GetRequiredService<IOptions<CardServerConfig>>().Value;
    // Log.Information("Card server config: {@Config}", config.ToString());
    
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms, " +
                                  "request host: {RequestHost}";
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        };
    });

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // auto start migration
	using (var scope = app.Services.CreateScope())
	{
		var db = scope.ServiceProvider.GetRequiredService<ServerDbContext>();
		db.Database.Migrate();
	}

    app.UseBlazorFrameworkFiles();
	app.MapControllers();
    app.UseStaticFiles();
    app.MapFallbackToFile("index.html");
    app.UseStatusCodePages(context =>
    {
        var code = context.HttpContext.Response.StatusCode;
        if (code == 404)
        {
            app.Logger.LogWarning("Request to {Path} returned 404, type is {Type}", 
                context.HttpContext.Request.Path,
                context.HttpContext.Request.Method);
        }

        return Task.CompletedTask;
    });
    
    app.Run();
}
catch (Exception ex) when (
    // https://github.com/dotnet/runtime/issues/60600
    ex.GetType().Name is not "StopTheHostException"
    // HostAbortedException was added in .NET 7
    // need to do it this way until we target .NET 8
    && ex.GetType().Name is not "HostAbortedException")
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}