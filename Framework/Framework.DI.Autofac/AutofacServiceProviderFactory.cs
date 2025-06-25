using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Framework.CQRS.Queries;
using Framework.CQRS.Commands;
using Framework.CQRS.Implementation;
using Framework.Database;
using Framework.Database.MongoDB;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Framework.DI.Autofac;

public class AutofacServiceProviderFactory : IServiceProviderFactory<IServiceCollection>
{
    public static IServiceProviderFactory<IServiceCollection> GetInstance => new AutofacServiceProviderFactory();
    
    public IServiceCollection CreateBuilder(IServiceCollection services) => services;

    public IServiceProvider CreateServiceProvider(IServiceCollection services)
    {
        var assembly = Assembly.Load("Application");
        var builder = new ContainerBuilder();
        
        builder.Populate(services);
        
        builder.RegisterAssemblyTypes(assembly)
            .AsClosedTypesOf(typeof(ICommandHandler<>))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(assembly)
            .AsClosedTypesOf(typeof(IQueryHandler<,>))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<CommandBus>().As<ICommandBus>().SingleInstance();
        builder.RegisterType<QueryBus>().As<IQueryBus>().SingleInstance();
        
        
        builder.RegisterType<RepositoryFactory>().As<IRepositoryFactory>().SingleInstance();
        
        
        string connectionString = "mongodb://root:example@localhost:27017";
        string databaseName = "generic";
        
        var mongoClient = new MongoClient(connectionString);
        builder.Register<IMongoDatabase>(x => mongoClient.GetDatabase(databaseName));
        
        return new AutofacServiceProvider(builder.Build());
    }
}