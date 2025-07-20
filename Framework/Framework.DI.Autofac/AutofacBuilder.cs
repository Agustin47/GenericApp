using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Framework.CQRS.Queries;
using Framework.CQRS.Commands;
using Framework.CQRS.Implementation;
using Framework.Database;
using Framework.Database.MongoDB;
using Framework.Domain.Events;
using Framework.EventManager;
using Framework.EventManager.MongoDb;
using Framework.Security;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Framework.DI.Autofac;


public class AutofacBuilder : IAutofacBuilder, IServiceProviderFactory<IServiceCollection>
{
    private readonly ContainerBuilder  _builder;

    private AutofacBuilder(ContainerBuilder builder)
    {
        _builder = builder;
    }
    
    public static IAutofacBuilder Start()
    {
        var builder = new ContainerBuilder();
        return new AutofacBuilder(builder);
    }

    public IAutofacBuilder AddDomain()
    {
        var assembly = Assembly.Load("Domain");
        
        _builder.RegisterAssemblyTypes(assembly)
            .AsClosedTypesOf(typeof(IEvent<>))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        
        _builder.RegisterType<DomainEntityFactory>().As<IDomainEntityFactory>().SingleInstance();
        return this;
    }

    public IAutofacBuilder AddCqrs(string? assemblyName = "Application")
    {
        var assembly = Assembly.Load(assemblyName);
        
        _builder.RegisterAssemblyTypes(assembly)
            .AsClosedTypesOf(typeof(ICommandHandler<>))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        
        _builder.RegisterAssemblyTypes(assembly)
            .AsClosedTypesOf(typeof(ICommandValidator<>))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        _builder.RegisterAssemblyTypes(assembly)
            .AsClosedTypesOf(typeof(ICommandPermissionValidator<>))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        
        _builder.RegisterAssemblyTypes(assembly)
            .AsClosedTypesOf(typeof(IQueryHandler<,>))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        
        _builder.RegisterAssemblyTypes(assembly)
            .AsClosedTypesOf(typeof(IQueryValidator<,>))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        
        _builder.RegisterAssemblyTypes(assembly)
            .AsClosedTypesOf(typeof(IQueryPermissionValidator<,>))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        
        _builder.RegisterType<CommandBus>().As<ICommandBus>().SingleInstance();
        _builder.RegisterType<QueryBus>().As<IQueryBus>().SingleInstance();
        
        return this;
    }

    public IAutofacBuilder AddMongoDd(IMongoOptions options)
    {
        _builder.RegisterType<RepositoryFactory>().As<IRepositoryFactory>().SingleInstance();
        
        var mongoClient = new MongoClient(options.ConnectionString);
        _builder.Register<IMongoDatabase>(x => mongoClient.GetDatabase(options.DatabaseName));

        return this;
    }

    public IAutofacBuilder AddSecurity(ISecurityOptions options)
    {
        _builder.Register(s => options).As<ISecurityOptions>().SingleInstance();
        _builder.RegisterType<AuthenticationMiddleware>();
        _builder.RegisterType<SecurityService>().As<ISecurityService>().SingleInstance();
        return this;
    }

    public IAutofacBuilder AddEventManager(string? assemblyName = "Application")
    {
        var assembly = Assembly.Load(assemblyName);
        _builder.RegisterAssemblyTypes(assembly)
            .AsClosedTypesOf(typeof(IEventProjection<>))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        
        _builder.RegisterType<EventDbFactory>().As<IEventDbFactory>().SingleInstance();
        _builder.RegisterType<EventIndexDbMongo>().As<IEventIndexDb>().SingleInstance();
        _builder.RegisterType<EventManager.EventManager>().As<IEventManager>().InstancePerLifetimeScope();
        return this;
    }

    public IServiceProviderFactory<IServiceCollection> Build() => this;
    
    public IServiceCollection CreateBuilder(IServiceCollection services) => services;

    public IServiceProvider CreateServiceProvider(IServiceCollection services)
    {
        _builder.Populate(services);
        var container = _builder.Build();
        return new AutofacServiceProvider(container);
    }
}

public interface IAutofacBuilder
{
    IAutofacBuilder AddDomain();
    IAutofacBuilder AddCqrs(string? assemblyName = "Application");
    IAutofacBuilder AddMongoDd(IMongoOptions  options);
    IAutofacBuilder AddSecurity(ISecurityOptions options);
    IAutofacBuilder AddEventManager(string? assemblyName = "Application");
    IServiceProviderFactory<IServiceCollection> Build();
}