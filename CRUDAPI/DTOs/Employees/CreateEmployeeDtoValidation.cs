using FluentValidation;

namespace CRUDAPI.DTOs.Employees
{
    public class CreateEmployeeDtoValidation :AbstractValidator<CreateEmployeeDto>
    {
        public CreateEmployeeDtoValidation() {
            RuleFor(x => x.Name).NotEmpty().WithMessage("name is required!");
            RuleFor(x => x.Description).MinimumLength(10).WithMessage("Description length error");
        
        }

    }
}
