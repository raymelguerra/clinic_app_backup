
using ClinicApp.Infrastructure.Dtos.Application;
using MediatR;

namespace ClinicApp.Infrastructure.Commands
{
    public record ContractorCreateCommand(CreateContractorRequest data) : IRequest<CreateContractorResponse>;
}
