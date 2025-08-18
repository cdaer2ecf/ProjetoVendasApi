using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleValidator : AbstractValidator<UpdateSaleCommand>
    {
        public UpdateSaleValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.CustomerName).NotEmpty();
            RuleFor(x => x.BranchId).NotEmpty();
            RuleFor(x => x.BranchName).NotEmpty();
        }
    }
}
