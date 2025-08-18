using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::Ambev.DeveloperEvaluation.Domain.Entities;
using global::Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

 
namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
  

    public sealed class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _ctx;

        public SaleRepository(DefaultContext ctx) => _ctx = ctx;

        public async Task AddAsync(Sale sale, CancellationToken ct)
        {
            await _ctx.Sales.AddAsync(sale, ct);
        }

        public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            // Include nos itens (OwnsMany) precisa ser feito com OwnsMany path
            return await _ctx.Sales
                .Include("_items")
                .FirstOrDefaultAsync(s => s.Id == id, ct);
        }

        public Task UpdateAsync(Sale sale, CancellationToken ct)
        {
            _ctx.Sales.Update(sale);
            return Task.CompletedTask;
        }

        public IQueryable<Sale> Query(Expression<Func<Sale, bool>>? predicate = null)
        {
            var query = _ctx.Sales.AsQueryable().Include("_items");
            if (predicate is not null) query = query.Where(predicate);
            return query;
        }
    }

}
