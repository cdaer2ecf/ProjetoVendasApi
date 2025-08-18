using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

 
namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
   

    public sealed class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
    {
        public CreateSaleCommandValidator()
        {
            RuleFor(x => x.Number).NotEmpty().MaximumLength(30);
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.CustomerName).NotEmpty().MaximumLength(150);
            RuleFor(x => x.BranchId).NotEmpty();
            RuleFor(x => x.BranchName).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Items).NotEmpty().WithMessage("At least one item is required.");

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId).NotEmpty();
                item.RuleFor(i => i.ProductName).NotEmpty().MaximumLength(200);
                item.RuleFor(i => i.UnitPrice).GreaterThanOrEqualTo(0);
                item.RuleFor(i => i.Quantity).GreaterThan(0);
            });
        }
    }

}
