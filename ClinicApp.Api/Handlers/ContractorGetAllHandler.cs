using AutoMapper;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Commands;
using ClinicApp.Infrastructure.Dtos.Application;
using ClinicApp.Infrastructure.Interfaces;
using ClinicApp.Infrastructure.Queries;
using MediatR;

namespace ClinicApp.Api.Handlers
{
    public class ContractorGetAllHandler : IRequestHandler<ContractorGetAllQuery, IQueryable<GetAllContractorsResponse>>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public ContractorGetAllHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IQueryable<GetAllContractorsResponse>> Handle(ContractorGetAllQuery request, CancellationToken cancellationToken)
        {
            var ctrList = await _repository.GetAllAsync<Contractor>();
            return ctrList.Select(c => _mapper.Map<GetAllContractorsResponse>(c)).AsQueryable();
        }
    }
}
