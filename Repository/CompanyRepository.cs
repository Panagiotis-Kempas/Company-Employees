using Contracts;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CompanyRepository : RepositoryBase<Company>,ICompanyRepository
    {
        public CompanyRepository(RepositoryContext context):base(context)
        {

        }

        public IEnumerable<Company> GetAllCompanies(bool trackChanges)
        {
           return FindAll(trackChanges).OrderBy(x=> x.Name).ToList();
        }

        public Company GetCompany(Guid companyId, bool trackChanges)
        {
            return FindByCondition(x => x.Id.Equals(companyId),trackChanges).SingleOrDefault();
        }
    }
}
