using Archiboard.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Configure Neo4j
var neo4jUri = builder.Configuration["Neo4j:Uri"] ?? "bolt://localhost:7687";
var neo4jUsername = builder.Configuration["Neo4j:Username"] ?? "neo4j";
var neo4jPassword = builder.Configuration["Neo4j:Password"] ?? "password";

// Register data store - use Neo4j if available, otherwise in-memory
try
{
    builder.Services.AddSingleton<IDataStore>(sp => new Neo4jDataStore(neo4jUri, neo4jUsername, neo4jPassword));
}
catch
{
    // Fallback to in-memory if Neo4j is not available
    builder.Services.AddSingleton<IDataStore, InMemoryDataStore>();
}

// Configure CORS for frontend
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost:80") // Angular default dev port and docker
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
