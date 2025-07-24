using Framework.DI.Autofac;
using Framework.Security;
using GenericWebApp;
using GenericWebApp.Middlewares;
using GenericWebApp.Modules;
using GenericWebApp.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwagger();

var mongoOptions = new MongoOptions();
builder.Configuration.GetSection(nameof(MongoOptions)).Bind(mongoOptions);

var securityOptions = new SecurityOptions();
builder.Configuration.GetSection(nameof(SecurityOptions)).Bind(securityOptions);
    
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddHostedService<AutomaticStarter>();

var provider = AutofacBuilder
    .Start()
    .AddDomain()
    .AddCqrs()
    .AddMongoDd(mongoOptions)
    .AddSecurity(securityOptions)
    .AddEventManager()
    .Build();

builder.Host.UseServiceProviderFactory(provider);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    app.MapOpenApi();
    
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger UI Modified V.2");
        c.RoutePrefix = string.Empty;
    });
}

app.UseMiddleware<AuthenticationMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();