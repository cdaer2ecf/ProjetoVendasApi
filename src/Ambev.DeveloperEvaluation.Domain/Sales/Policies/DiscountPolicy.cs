using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Policies
{
    public static class DiscountPolicy
    {
        /// <summary>
        /// Retorna a % de desconto com base na quantidade.
        /// 1–3 => 0% ; 4–9 => 10% ; 10–20 => 20% ; >20 => inválido.
        /// </summary>
        public static decimal ForQuantity(int quantity)
        {
            if (quantity < 1) throw new DomainException("Quantity must be >= 1");
            if (quantity <= 3) return 0m;
            if (quantity <= 9) return 10m;
            if (quantity <= 20) return 20m;
            throw new DomainException("Quantity cannot exceed 20");
        }
    }

    /// <summary>
    /// Exceção de domínio simples. Se seu template já tem uma DomainException, remova essa classe e use a existente.
    /// </summary>
    public sealed class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }
    }
}
