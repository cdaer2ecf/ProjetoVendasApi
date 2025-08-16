using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using AutoMapper;
using global::Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
         

    public sealed class GetSaleProfile : Profile
    {
        public GetSaleProfile()
        {
            CreateMap<GetSaleResult, GetSaleResponse>();
            CreateMap<GetSaleItemResult, GetSaleItemResponse>();
        }
    }

}
