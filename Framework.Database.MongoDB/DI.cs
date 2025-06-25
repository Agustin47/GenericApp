// using Autofac;
// using MongoDB.Driver;
// using System.Reflection;
//
// namespace Framework.Database.MongoDB;
//
// public static class DI
// {
//     public static ContainerBuilder AddMongoDatabase(this ContainerBuilder builder)
//     {
//         var assembly = Assembly.Load("Framework.Database.MongoDB");
//
//         builder.RegisterAssemblyTypes(assembly)
//             .AsClosedTypesOf(typeof(IRepository<>))
//             .AsImplementedInterfaces()
//             .SingleInstance();
//         
//         
//         string connectionString = "mongodb://root:example@mongo:27017/";
//         string databaseName = "generic";
//         
//         var mongoClient = new MongoClient(connectionString);
//         builder.Register<IMongoDatabase>(x => mongoClient.GetDatabase(databaseName));
//         
//         return builder;
//     }
// }