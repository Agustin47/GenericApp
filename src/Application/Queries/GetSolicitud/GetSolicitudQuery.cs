using Domain.Aggregates;
using Framework.CQRS.Implementation;

namespace Application.Queries.GetSolicitud;

public class GetSolicitudQuery : QueryBase<List<Solicitud>>;
