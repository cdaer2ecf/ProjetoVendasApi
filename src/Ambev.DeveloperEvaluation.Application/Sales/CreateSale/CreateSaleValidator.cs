using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleValidator : AbstractValidator<CreateSaleCommand>
    {
        public CreateSaleValidator()
        {
            RuleFor(x => x.Number).NotEmpty();
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.CustomerName).NotEmpty();
            RuleFor(x => x.BranchId).NotEmpty();
            RuleFor(x => x.BranchName).NotEmpty();
            RuleForEach(x => x.Items).SetValidator(new CreateSaleItemValidator());
        }
    }

    public class CreateSaleItemValidator : AbstractValidator<CreateSaleItem>
    {
        public CreateSaleItemValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.ProductName).NotEmpty();
            RuleFor(x => x.UnitPrice).GreaterThan(0);
            RuleFor(x => x.Quantity).InclusiveBetween(1, 20);
        }
    }
}
