using AutoMapper;
using Contracts;
using Entities.Exceptions.CompanyExceptions;
using Entities.Models;
using ServicesContracts;
using Shared.DataTransferObjects.CompanyDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public CompanyService(IRepositoryManager repository, ILoggerManager logger,IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
        {           
                var companies = _repository.Company.GetAllCompanies(trackChanges);

                var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companies);

                return companiesToReturn;        
        }

        public CompanyDto GetCompany(Guid companyId,bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges);

            if (company is null)
                throw new CompanyNotFoundException(companyId);


            var companyToReturn = _mapper.Map<CompanyDto>(company);

            return companyToReturn;
        }
    }
}
