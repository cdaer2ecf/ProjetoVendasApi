using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using global::Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

 
namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
   

    public sealed class GetSaleHandler : IRequestHandler<GetSaleQuery, GetSaleResult>
    {
        private readonly ISaleRepository _repo;
        private readonly IMapper _mapper;

        public GetSaleHandler(ISaleRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<GetSaleResult> Handle(GetSaleQuery request, CancellationToken ct)
        {
            var sale = await _repo.GetByIdAsync(request.Id, ct)
                       ?? throw new KeyNotFoundException("Sale not found");
            return _mapper.Map<GetSaleResult>(sale);
        }
    }

}
