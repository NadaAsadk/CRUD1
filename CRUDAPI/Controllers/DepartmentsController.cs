using CRUDAPI.Data;
using CRUDAPI.DTOs;
using CRUDAPI.DTOs.Departments;
using CRUDAPI.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUDAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly ApplicationDBContext context;

        private readonly ILogger<DepartmentsController> logger;
        public DepartmentsController(ApplicationDBContext context, ILogger<DepartmentsController> logger) {
            this.context = context;
            this.logger = logger;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
                var departments = await context.Departments.Select(
                x => new GetDepartmentsDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                }
                ).ToListAsync();
                return Ok(departments);
        }
        [HttpGet("details")]
        public async Task<IActionResult> GetByID(int id)
        {
            var dep = await context.Departments.FindAsync(id);
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
        public async Task<IActionResult> Create(CreateDepartmentDto depDto)
        {
            Department dep = new Department()
            {
                Name = depDto.Name
            };
            await context.Departments.AddAsync(dep);
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(int id, UpdateDepartmentDto depDto)
        {
            var entity = await context.Departments.FindAsync(id);
            if (entity is null)
                return NotFound("Employee Not Found");

            entity.Name = depDto.Name;
            
            await context.SaveChangesAsync();

            return Ok(depDto);
        }
        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove(int id)
        {
            var entity = await context.Departments.FindAsync(id);
            if (entity is null)
                return NotFound("Department Not Found");

            context.Departments.Remove(entity);
            var res = new RemoveDepartmentDto()
            {
                Id = entity.Id,
                Name = entity.Name
            };
            await context.SaveChangesAsync();

            return Ok(res);
        }
    }
}
