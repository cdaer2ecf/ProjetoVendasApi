using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
  

 
    public sealed class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
    {
        public CreateSaleRequestValidator()
        {
            RuleFor(x => x.Number).NotEmpty().MaximumLength(30);
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.CustomerName).NotEmpty().MaximumLength(150);
            RuleFor(x => x.BranchId).NotEmpty();
            RuleFor(x => x.BranchName).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Items).NotEmpty();

            RuleForEach(x => x.Items).ChildRules(i =>
            {
                i.RuleFor(m => m.ProductId).NotEmpty();
                i.RuleFor(m => m.ProductName).NotEmpty().MaximumLength(200);
                i.RuleFor(m => m.UnitPrice).GreaterThanOrEqualTo(0);
                i.RuleFor(m => m.Quantity).GreaterThan(0);
            });
        }
    }

}
