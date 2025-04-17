using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using Serilog;
using Server.Common.Validation;
using Server.Jobs;
using Server.Middlewares;
using Server.Models.Config;
using Server.Persistence;

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

    builder.Services.AddOptions<CardServerConfig>()
        .Bind(builder.Configuration.GetSection(CardServerConfig.CARD_SERVER_SECTION))
        .ValidateMiniValidation()
        .ValidateOnStart();

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
            new SqliteConnectionStringBuilder() { DataSource = "Server.db" }.ConnectionString,
            o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
        )
    );
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    
    builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
    builder.Services.AddQuartz(quartz =>
    {
        quartz.UseMicrosoftDependencyInjectionJobFactory();
        
        var patchOfflineBattleJobKey = new JobKey("PatchOfflineBattleJob", "PatchOfflineBattleJob");
        quartz.AddJob<PatchOfflineBattleJob>(opts =>
        {
            opts.WithIdentity(patchOfflineBattleJobKey);
            opts.StoreDurably();
        });
        
        var consolidateServerOfflineSnapshotJobKey = new JobKey("ConsolidateServerOfflineSnapshotJob", "ConsolidateServerOfflineSnapshotJob");
        quartz.AddJob<ConsolidateServerOfflineSnapshotJob>(opts =>
        {
            opts.WithIdentity(consolidateServerOfflineSnapshotJobKey);
            opts.StoreDurably();
        });
        
        // Data Patching is at 4am JPT
        // This is mandatory is you have existing data
        // And if you wish to perform consolidation later
        quartz.AddTrigger(opts =>
        {
            opts.ForJob(patchOfflineBattleJobKey);
            opts.WithIdentity("PatchOfflineBattleJob", "PatchOfflineBattleJob");
            opts.WithCronSchedule(
                "0 0 4 ? * * *",
                x => x.InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time"))
            );
        });
        
        // Server-wise Stat Consolidation is at 5am JPT
        // For 1st time, it need the result of PatchOfflineBattleJob
        // For 2nd time or later, it is incremental update based on update
        quartz.AddTrigger(opts =>
        {
            opts.ForJob(consolidateServerOfflineSnapshotJobKey);
            opts.WithIdentity("ConsolidateServerOfflineSnapshotJob", "ConsolidateServerOfflineSnapshotJob");
            opts.WithCronSchedule(
                "0 0 5 ? * * *",
                x => x.InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time"))
            );
        });
    });

    var app = builder.Build();
    
    var config = app.Services.GetRequiredService<IOptions<CardServerConfig>>().Value;
    Log.Information("Card server config: {@Config}", config.ToString());
    
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
    app.UseWhen(
        context => context.Request.Path.StartsWithSegments("/sys/servlet/PowerOn", StringComparison.InvariantCulture),
        applicationBuilder => applicationBuilder.UseAllNetRequestMiddleware());
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