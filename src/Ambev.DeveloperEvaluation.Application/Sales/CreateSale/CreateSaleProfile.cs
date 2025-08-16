using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
using AutoMapper;
using global::Ambev.DeveloperEvaluation.Domain.Entities;
 using AutoMapper;
namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{




 

    public sealed class CreateSaleProfile : Profile
    {
        public CreateSaleProfile()
        {
            CreateMap<CreateSaleCommand, Sale>()
                .ConstructUsing(c => new Sale(
                    Guid.NewGuid(),
                    c.Number,
                    c.Date,
                    c.CustomerId,
                    c.CustomerName,
                    c.BranchId,
                    c.BranchName
                ))
                // ⚠️ chave do problema: NÃO deixe o AutoMapper mexer na coleção
                .ForMember(d => d.Items, opt => opt.Ignore());

            // Este mapeamento pode até ficar, mas não será usado no fluxo atual.
            CreateMap<CreateSaleItem, SaleItem>()
                .ConstructUsing(i => new SaleItem(
                    i.ProductId,
                    i.ProductName,
                    i.UnitPrice,
                    i.Quantity
                ));

            CreateMap<Sale, CreateSaleResult>()
                .ForMember(d => d.Total, opt => opt.MapFrom(s => s.Total));
        }
    }

}
