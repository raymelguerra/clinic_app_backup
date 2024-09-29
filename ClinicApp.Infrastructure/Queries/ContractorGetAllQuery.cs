
using ClinicApp.Infrastructure.Dtos.Application;
using MediatR;

namespace ClinicApp.Infrastructure.Queries
{
    public record ContractorGetAllQuery : IRequest<IQueryable<GetAllContractorsResponse>>;
}
