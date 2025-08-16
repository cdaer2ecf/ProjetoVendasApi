using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using AutoMapper;
using global::Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
   

 
    public sealed class CreateSaleProfile : Profile
    {
        public CreateSaleProfile()
        {
            CreateMap<CreateSaleRequest, CreateSaleCommand>();
            CreateMap<CreateSaleItemRequest, CreateSaleItem>();
            CreateMap<CreateSaleResult, CreateSaleResponse>();
        }
    }

}
