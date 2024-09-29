
using ClinicApp.Infrastructure.Dtos.Application;
using MediatR;

namespace ClinicApp.Infrastructure.Queries
{
    public record ContractorGetByIdQuery(int contractorId) : IRequest<GetContractorByIdResponse>;
}
