using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using global::Ambev.DeveloperEvaluation.Domain.Entities;
using global::Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

 
namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
  

    public sealed class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _repo;
        private readonly IMapper _mapper;
        private readonly DbContext _ctx; // usa o DefaultContext via DI

        public CreateSaleHandler(ISaleRepository repo, IMapper mapper, DbContext ctx)
        {
            _repo = repo;
            _mapper = mapper;
            _ctx = ctx;
        }

        public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken ct)
        {
            new CreateSaleCommandValidator().ValidateAndThrow(request);

            var sale = _mapper.Map<Sale>(request);

            foreach (var it in request.Items)
            {
                sale.AddItem(it.ProductId, it.ProductName, it.UnitPrice, it.Quantity);
            }

            await _repo.AddAsync(sale, ct);
            await _ctx.SaveChangesAsync(ct); // simples e direto

            return _mapper.Map<CreateSaleResult>(sale);
        }
    }

}
