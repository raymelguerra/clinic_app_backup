
using ClinicApp.Infrastructure.Dtos.Application;
using MediatR;

namespace ClinicApp.Infrastructure.Commands
{
    public record ContractorUpdateCommand(int contracttorId, UpdateContractorRequest data) : IRequest<UpdateContractorResponse>;
}
