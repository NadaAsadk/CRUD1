using CRUDAPI.Data;
using CRUDAPI.DTOs.Employees;
using CRUDAPI.Models;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CRUDAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ploicy")]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDBContext context;

        public EmployeesController(ApplicationDBContext context)
        {
            this.context = context;
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var employees = context.Employees.ToList();
            var response = employees.Adapt<IEnumerable<GetEmployeeDto>>();
            return Ok(response);
        }
        [HttpGet("details")]
        public IActionResult GetByID(int id)
        {
            var employee = context.Employees.Find(id);
            var response = employee.Adapt<GetEmployeeBYIdDto> ();
            if (employee is null)
                return NotFound();
            return Ok(response);
        }
        [HttpPost("Create")]
        public IActionResult Create(CreateEmployeeDto empDto,[FromServices] IValidator<CreateEmployeeDto> validator)
        {
            var validationResult = validator.Validate(empDto);
            if (!validationResult.IsValid)
            {
                var modelState = new ModelStateDictionary();
                foreach (var item in validationResult.Errors)
                {
                    modelState.AddModelError(item.PropertyName,item.ErrorMessage);
                }
                return ValidationProblem();
            }
            var emp = empDto.Adapt<Employee>();
            context.Employees.Add(emp); 
            context.SaveChanges();

            return Ok();
        }

        [HttpPut("Update")]
        public IActionResult Update(int id, UpdateEmployeeDto emp)
        {
            var entity = context.Employees.Find(id);
            if (entity is null)
                return NotFound("Employee Not Found");

            entity.Name = emp.Name;
            entity.Description = emp.Description;

            var res = entity.Adapt<UpdateEmployeeDto>();
            
            context.SaveChanges();

            return Ok(emp);
        }
        [HttpDelete("Remove")]
        public IActionResult Remove(int id)
        {
            var entity = context.Employees.Find(id);
            if (entity is null)
                return NotFound("Employee Not Found");
            context.Employees.Remove(entity);
            context.SaveChanges();
            var emp = entity.Adapt<RemoveEmployeeDto>();

            return Ok(emp);
        }

    }
}
