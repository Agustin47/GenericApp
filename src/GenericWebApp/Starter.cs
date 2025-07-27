using Application.Roles;
using Domain;
using Domain.Aggregates;
using Framework.Common;
using Framework.Database;
using Framework.Domain;
using Framework.Security;

namespace GenericWebApp;

public class AutomaticStarter(ISecurityService securityService,
    IRepositoryFactory repositoryFactory,
    IDomainEntityFactory domainEntityFactory,
    ILogger<AutomaticStarter> logger)
    : IHostedService
{
    private const string Username = "admin";
    private const string Password = "admin";
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting automatic starter");
        
        var initRepo = repositoryFactory.GetRepository<Starter>();
        var query = QueryRepositoryBuilder<Starter>.Create().Build();
        var init = await initRepo.FirstOrDefault(query);
        var initvalue = init.Value;
        if (init.IsFailed || initvalue != null)
            return;

        if (initvalue == null)
            initvalue = new()
            {
                DateTime = DateTime.UtcNow,
                Id = Guid.NewGuid()
            };
        
        await DBInit();
        await initRepo.CreateAsync(initvalue);
        
        var login = await securityService.Login(Username, Password);
        if (!login.IsFailed)
            return;

        var newUserRoles = Rol.UserManager.Permissions.Select(x => x.Name).ToArray();
        await securityService.RegisterUser(Username, Password, string.Empty, Username, Username,
            Rol.UserManager.Name, newUserRoles);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private async Task DBInit()
    {
        foreach (dynamic comuna in Comunas())
        {
            EntityId id = new(Guid.NewGuid());
            var comunaEntity = domainEntityFactory.Create<Comuna>(id);
            comunaEntity.New(comuna.nombre, comuna.codigo, comuna.estado, comuna.poblacion,
                nameof(AutomaticStarter));

            var oficinas = Oficinas().Where(x => x.comunaId == comuna.id);
            EntityId oId = new(Guid.NewGuid());
            foreach (dynamic oficina in oficinas)
            {
                var oficinaEntity = domainEntityFactory.Create<Oficina>(oId);
                oficinaEntity.New(oficina.nombre, oficina.codigo, comunaEntity.Id, comunaEntity.Nombre,
                    oficina.direccion, oficina.telefono, oficina.estado,
                    nameof(AutomaticStarter));
                
                comunaEntity.AddOficina(oficinaEntity);
                await oficinaEntity.SaveChanges();
            }
            await comunaEntity.SaveChanges();
        }


    }

    private IEnumerable<dynamic> Oficinas()
        =>
        [
            new
            {
                id = 1,
                nombre = "Norte 1",
                codigo = "ON1-001",
                comuna = "Norte",
                comunaId = 1,
                direccion = "Av. Principal 123",
                telefono = "+56 2 1234 5678",
                usuarios = 4,
                estado = "Activa",
            },
            new
            {
                id = 2,
                nombre = "Norte 2",
                codigo = "ON2-002",
                comuna = "Norte",
                comunaId = 1,
                direccion = "Calle Secundaria 456",
                telefono = "+56 2 1234 5679",
                usuarios = 4,
                estado = "Activa",
            },
            new
            {
                id = 3,
                nombre = "Sur Central",
                codigo = "OSC-003",
                comuna = "Sur",
                comunaId = 2,
                direccion = "Plaza Central 789",
                telefono = "+56 2 1234 5680",
                usuarios = 5,
                estado = "Activa",
            },
            new
            {
                id = 4,
                nombre = "Centro",
                codigo = "OC-004",
                comuna = "Centro",
                comunaId = 3,
                direccion = "Av. Libertador 321",
                telefono = "+56 2 1234 5681",
                usuarios = 8,
                estado = "Activa",
            },
            new
            {
                id = 5,
                nombre = "Este",
                codigo = "OE-005",
                comuna = "Este",
                comunaId = 4,
                direccion = "Calle Nueva 654",
                telefono = "+56 2 1234 5682",
                usuarios = 3,
                estado = "Mantenimiento",
            },
        ];
    
    private IEnumerable<dynamic> Comunas()
        =>
        [
            new
            {
                id = 1,
                nombre = "Norte",
                codigo = "CN-001",
                oficinas = 3,
                usuarios = 12,
                poblacion = 45000,
                estado = "Activa",
                coordinador = "Ana Pérez",
            },
            new
            {
                id = 2,
                nombre = "Sur",
                codigo = "CS-002",
                oficinas = 2,
                usuarios = 8,
                poblacion = 38000,
                estado = "Activa",
                coordinador = "Luis Martín",
            },
            new
            {
                id = 3,
                nombre = "Centro",
                codigo = "CC-003",
                oficinas = 4,
                usuarios = 15,
                poblacion = 52000,
                estado = "Activa",
                coordinador = "Carmen Silva",
            },
            new
            {
                id = 4,
                nombre = "Este",
                codigo = "CE-004",
                oficinas = 2,
                usuarios = 6,
                poblacion = 28000,
                estado = "Activa",
                coordinador = "Roberto Díaz",
            },
        ];

    public class Starter : IEntity
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
    }
}