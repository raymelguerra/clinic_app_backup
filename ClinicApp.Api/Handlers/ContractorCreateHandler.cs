using AutoMapper;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Commands;
using ClinicApp.Infrastructure.Dtos.Application;
using ClinicApp.Infrastructure.Interfaces;
using MediatR;

namespace ClinicApp.Api.Handlers
{
    public class ContractorCreateHandler : IRequestHandler<ContractorCreateCommand, CreateContractorResponse>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        // Constructor estándar
        public ContractorCreateHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CreateContractorResponse> Handle(ContractorCreateCommand request, CancellationToken cancellationToken)
        {
            var ctr = _mapper.Map<Contractor>(request.data);
            await _repository.AddAsync(ctr);

            return _mapper.Map<CreateContractorResponse>(ctr);
        }
    }
}
