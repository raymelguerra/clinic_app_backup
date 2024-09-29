
using ClinicApp.Infrastructure.Dtos.Application;
using MediatR;

namespace ClinicApp.Infrastructure.Commands
{
    public record ContractorDeleteCommand(int contractorId) : IRequest<bool>;
}
