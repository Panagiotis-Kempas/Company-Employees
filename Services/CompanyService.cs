using AutoMapper;
using Contracts;
using Entities.Exceptions.CompanyExceptions;
using Entities.Models;
using ServicesContracts;
using Shared.DataTransferObjects.CompanyDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

        public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company)
        {
            var companyEntity = _mapper.Map<Company>(company);

             _repository.Company.CreateCompany(companyEntity);

            await _repository.SaveAsync();

            var companyDto = _mapper.Map<CompanyDto>(companyEntity);

            return companyDto;
        }

        public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companyCollection)
        {
            if (companyCollection == null)
                throw new CompanyCollectionBadRequest();

            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);

            foreach (var company in companyEntities)
            {
                _repository.Company.CreateCompany(company);
            }
            await _repository.SaveAsync();

            var companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);

            var ids = string.Join(",", companyCollectionToReturn.Select(company => company.Id));

            return (companies: companyCollectionToReturn, ids);
        }

        public async Task DeleteCompanyAsync(Guid companyId, bool trackChanges)
        {
            var company = await GetCompanyAndCheckIfExists(companyId, trackChanges);
            _repository.Company.DeleteCompany(company);

           await  _repository.SaveAsync();
        }

        public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges)
        {           
                var companies = await _repository.Company.GetAllCompaniesAsync(trackChanges);

                var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companies);

                return companiesToReturn;        
        }

        public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();

            var companiesEntities =await  _repository.Company.GetByIdsAsync(ids, trackChanges);
            if (ids.Count() != companiesEntities.Count())
                throw new CollectionByIdsBadRequestException();

            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companiesEntities);

            return companiesToReturn;
        }

        public async Task<CompanyDto> GetCompanyAsync(Guid companyId,bool trackChanges)
        {
            var company = await GetCompanyAndCheckIfExists(companyId, trackChanges);

            var companyToReturn = _mapper.Map<CompanyDto>(company);

            return companyToReturn;
        }

        public async Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges)
        {
            var company = await GetCompanyAndCheckIfExists(companyId, trackChanges);

            var companyEntity = _mapper.Map(companyForUpdate,company);
           await  _repository.SaveAsync();
        }

        private async Task<Company> GetCompanyAndCheckIfExists(Guid id,bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(id, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(id);
            return company;
        }
    }
}
