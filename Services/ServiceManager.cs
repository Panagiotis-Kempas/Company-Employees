using AutoMapper;
using Contracts;
using ServicesContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IEmployeeService> _employeeService;
        private readonly Lazy<ICompanyService> _companyService;

        public ServiceManager(IRepositoryManager repositoryManager,ILoggerManager logger,IMapper mapper)
        {
            _companyService = new Lazy<ICompanyService>(() => new CompanyService(repositoryManager, logger,mapper));
            _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(repositoryManager, logger,mapper));
        }
        public IEmployeeService Employee => _employeeService.Value;

        public ICompanyService Company => _companyService.Value;
    }
}
