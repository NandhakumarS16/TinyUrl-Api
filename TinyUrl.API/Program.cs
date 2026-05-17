var builder = WebApplication.CreateBuilder(args);

// Configure services
ProgramHelper.ConfigureServices(builder);

var app = builder.Build();

// Configure middleware pipeline
ProgramHelper.ConfigurePipeline(app);

// Map endpoints
app.MapTinyUrlEndpoints();

app.Run();