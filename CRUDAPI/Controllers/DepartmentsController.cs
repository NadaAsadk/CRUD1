using CRUDAPI.Data;
using CRUDAPI.DTOs;
using CRUDAPI.DTOs.Departments;
using CRUDAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRUDAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        ApplicationDBContext context = new ApplicationDBContext();
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var departments = context.Departments.Select(
                x => new GetDepartmentsDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                }
                );
            return Ok(departments);
        }
        [HttpGet("details")]
        public IActionResult GetByID(int id)
        {
            var dep = context.Departments.Find(id);
            if (dep is null)
                return NotFound();
            var response = new GetDepartmentsDto()
            {
                Id = dep.Id,
                Name = dep.Name,
            };
            return Ok(response);
        }
        [HttpPost("Create")]
        public IActionResult Create(CreateDepartmentDto depDto)
        {
            Department dep = new Department()
            {
                Name = depDto.Name
            };
            context.Departments.Add(dep);
            context.SaveChanges();

            return Ok();
        }

        [HttpPut("Update")]
        public IActionResult Update(int id, UpdateDepartmentDto depDto)
        {
            var entity = context.Departments.Find(id);
            if (entity is null)
                return NotFound("Employee Not Found");

            entity.Name = depDto.Name;
            
            context.SaveChanges();

            return Ok(depDto);
        }
        [HttpDelete("Remove")]
        public IActionResult Remove(int id)
        {
            var entity = context.Departments.Find(id);
            if (entity is null)
                return NotFound("Department Not Found");

            context.Departments.Remove(entity);
            var res = new RemoveDepartmentDto()
            {
                Id = entity.Id,
                Name = entity.Name
            };
            context.SaveChanges();

            return Ok(res);
        }
    }
}
