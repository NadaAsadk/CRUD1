using CRUDAPI.DTOs.Employees;
using FluentValidation;

namespace CRUDAPI.DTOs.Products
{
    public class CreateProductValidation : AbstractValidator<CreateProductDto>
    {
        public CreateProductValidation() {
            RuleFor(x => x.Name).NotEmpty().WithMessage("name is required!");
            RuleFor(x => x.Name).MinimumLength(5).WithMessage("MinimumLength for the name is 5!");
            RuleFor(x => x.Name).MaximumLength(10).WithMessage("MaximumLength for the name is 10!");
            RuleFor(x => x.Price).LessThan(3000).GreaterThan(20).WithMessage("Range for the Price Between 20, 3000");
            RuleFor(x => x.Description).MinimumLength(10).WithMessage("Description length error");
            RuleFor(x => x.Description).NotNull().WithMessage("Description is required");
        }
    }
}
