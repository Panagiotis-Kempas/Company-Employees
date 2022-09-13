using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _context;
        private readonly Lazy<IEmployeeRepository> _employeeRepository;
        private readonly Lazy<ICompanyRepository> _companyRepository;

        public RepositoryManager(RepositoryContext context)
        {
            _context = context;
            _employeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(context));
            _companyRepository = new Lazy<ICompanyRepository>(() => new CompanyRepository(context));
        }

        public IEmployeeRepository Employee => _employeeRepository.Value;

        public ICompanyRepository Company => _companyRepository.Value;

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
