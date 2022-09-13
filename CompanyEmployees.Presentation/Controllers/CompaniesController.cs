using Microsoft.AspNetCore.Mvc;
using ServicesContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _service;

        public CompaniesController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetCompanies()
        {
            var companies = _service.Company.GetAllCompanies(trackChanges: false);
            return Ok(companies);
        }

        [HttpGet("{Id:Guid}")]
        public IActionResult GetCompany(Guid Id)
        {
            var company = _service.Company.GetCompany(Id, trackChanges: false);
            return Ok(company);
        }
    }
}
