using FluentValidation;

namespace Product.Command.Application;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(user => user.Price).GreaterThanOrEqualTo(0).WithMessage("Price must positive number.");
    }

}
