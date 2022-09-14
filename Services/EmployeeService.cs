using AutoMapper;
using Contracts;
using Entities.Exceptions.CompanyExceptions;
using Entities.Exceptions.EmployeeExceptions;
using Entities.Models;
using ServicesContracts;
using Shared.DataTransferObjects.EmployeeDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
        {
            await CheckIfCompanyExists(companyId, trackChanges);

            var employeeEntity = _mapper.Map<Employee>(employeeForCreation);
            _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
            await _repository.SaveAsync();

            var employeeDto = _mapper.Map<EmployeeDto>(employeeEntity);
            return employeeDto;
        }

        public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges)
        {
            await CheckIfCompanyExists(companyId, trackChanges);

            var employeeForCompany = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);
            if (employeeForCompany is null)
                throw new EmployeeNotFoundException(id);

            _repository.Employee.DeleteEmployee(employeeForCompany);
            await _repository.SaveAsync();
        }

        public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
        {
            await CheckIfCompanyExists(companyId, trackChanges);

            var employee = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);

            if (employee is null)
                throw new EmployeeNotFoundException(id);

            var employeeToReturn = _mapper.Map<EmployeeDto>(employee);

            return employeeToReturn;
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid companyId, bool trackChanges)
        {
            await CheckIfCompanyExists(companyId, trackChanges);

            var employees = await _repository.Employee.GetEmployeesAsync(companyId, trackChanges);

            var employeesToReturn = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return employeesToReturn;
        }

        public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool compRrackChanges, bool emplTrackChanges)
        {
            await CheckIfCompanyExists(companyId, compRrackChanges);

            var employeeEntity = await _repository.Employee.GetEmployeeAsync(companyId, id, emplTrackChanges);
            if (employeeEntity is null)
                throw new EmployeeNotFoundException(id);
            _mapper.Map(employeeForUpdate, employeeEntity);
            await _repository.SaveAsync();
        }

        private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
        }

        private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExists(Guid companyId, Guid id, bool trackChanges)
        {
            var employeeDb = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);

            if (employeeDb is null)
                throw new EmployeeNotFoundException(id);
            return employeeDb;
        }
    }
}
