using CompanyEmployees.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using ServicesContracts;
using Shared.DataTransferObjects.EmployeeDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IServiceManager _service;
        public EmployeesController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task< IActionResult> GetEmployeesForCompany(Guid companyId)
        {
            var employees = await _service.Employee.GetEmployeesAsync(companyId, trackChanges: false);
            return Ok(employees);
        }

        [HttpGet("{id:guid}",Name = "GetEmployeeForCompany")]
        public async Task< IActionResult> GetEmployeeForCompany(Guid companyId,Guid id)
        {
            var employee = await _service.Employee.GetEmployeeAsync(companyId, id, trackChanges: false);
            return Ok(employee);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task< IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employeeForCreation)
        {
            var employeeToReturn = await _service.Employee.CreateEmployeeForCompanyAsync(companyId, employeeForCreation, trackChanges: false);

            return CreatedAtRoute("GetEmployeeForCompany", new { companyId, id = employeeToReturn.Id }, employeeToReturn);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId,Guid id)
        {
            await _service.Employee.DeleteEmployeeForCompanyAsync(companyId, id, trackChanges: false);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task< IActionResult> UpdateEmployeeForCompany(Guid companyId,Guid id, [FromBody] EmployeeForUpdateDto employeeForUpdate)
        {
            await _service.Employee.UpdateEmployeeForCompanyAsync(companyId, id, employeeForUpdate, compRrackChanges: false, emplTrackChanges: true);
            return NoContent();
        }
    }
}
