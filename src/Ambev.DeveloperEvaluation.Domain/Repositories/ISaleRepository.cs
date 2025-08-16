using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::Ambev.DeveloperEvaluation.Domain.Entities;
using System.Linq.Expressions;


namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    

    public interface ISaleRepository
    {
        Task AddAsync(Sale sale, CancellationToken ct);
        Task<Sale?> GetByIdAsync(Guid id, CancellationToken ct);
        Task UpdateAsync(Sale sale, CancellationToken ct);

        /// <summary>
        /// Retorna um IQueryable para compor filtros/paginação no Application.
        /// </summary>
        IQueryable<Sale> Query(Expression<Func<Sale, bool>>? predicate = null);
    }

}
