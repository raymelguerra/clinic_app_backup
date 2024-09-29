using AutoMapper;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Commands;
using ClinicApp.Infrastructure.Dtos.Application;
using ClinicApp.Infrastructure.Interfaces;
using ClinicApp.Infrastructure.Queries;
using MediatR;

namespace ClinicApp.Api.Handlers
{
    public class ContractorGetByIdHandler : IRequestHandler<ContractorGetByIdQuery, GetContractorByIdResponse>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public ContractorGetByIdHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetContractorByIdResponse> Handle(ContractorGetByIdQuery request, CancellationToken cancellationToken)
        {
            var ctr = await _repository.GetByIdAsync<Contractor>(request.contractorId);
            return _mapper.Map<GetContractorByIdResponse>(ctr);
        }
    }
}
