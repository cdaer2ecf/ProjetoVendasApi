using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using global::Ambev.DeveloperEvaluation.Domain.Entities;

 
namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
   

    public sealed class GetSaleProfile : Profile
    {
        public GetSaleProfile()
        {
            CreateMap<SaleItem, GetSaleItemResult>()
                .ForMember(d => d.Total, opt => opt.MapFrom(s => s.Total));

            CreateMap<Sale, GetSaleResult>()
                .ForMember(d => d.Total, opt => opt.MapFrom(s => s.Total))
                .ForMember(d => d.Items, opt => opt.MapFrom(s => s.Items));
        }
    }

}
