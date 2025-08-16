using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Requests;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Responses;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings
{
    public class SaleProfile : Profile
    {
        public SaleProfile()
        {
            CreateMap<CreateSaleRequest, CreateSaleCommand>();
            CreateMap<CreateSaleItemRequest, CreateSaleItem>();

            CreateMap<UpdateSaleRequest, UpdateSaleCommand>();

            // Queries -> Responses
            CreateMap<Ambev.DeveloperEvaluation.Application.Sales.GetSale.GetSaleItemResult,
              Ambev.DeveloperEvaluation.WebApi.Features.Sales.Responses.SaleItemResponse>();

            CreateMap<Ambev.DeveloperEvaluation.Application.Sales.GetSale.GetSaleResult,
                      Ambev.DeveloperEvaluation.WebApi.Features.Sales.Responses.SaleResponse>();


            CreateMap<ListSalesRow, ListSalesRowResponse>();
            CreateMap<ListSalesResult, ListSalesResponse>();
        }
    }
}
