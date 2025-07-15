    // Arquivo: SalesWebMvc/Services/SalesRecordService.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels; // Certifique-se de que este using existe

namespace SalesWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context;

        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task InsertAsync(SalesRecord obj)
        {
            _context.SalesRecord.Add(obj);
            await _context.SaveChangesAsync();
        }

        // Método para a busca simples de vendas por data (já modificado)
        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;

            if (minDate.HasValue)
            {
                var utcMinDate = minDate.Value.ToUniversalTime();
                result = result.Where(x => x.Date >= utcMinDate);
            }

            if (maxDate.HasValue)
            {
                var utcMaxDateInclusive = maxDate.Value.AddDays(1).ToUniversalTime();
                result = result.Where(x => x.Date < utcMaxDateInclusive);
            }

            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        // NOVO MÉTODO: Para a busca agrupada de vendas por data (FindByDateGroupingAsync)
        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;

            if (minDate.HasValue)
            {
                // CONVERSÃO CRÍTICA: Converta minDate para UTC
                var utcMinDate = minDate.Value.ToUniversalTime();
                result = result.Where(x => x.Date >= utcMinDate);
            }

            if (maxDate.HasValue)
            {
                // CONVERSÃO CRÍTICA: Converta maxDate para UTC e ajuste para incluir o dia inteiro final
                var utcMaxDateInclusive = maxDate.Value.AddDays(1).ToUniversalTime();
                result = result.Where(x => x.Date < utcMaxDateInclusive);
            }

            // A diferença aqui é o GroupBy. Agrupamos por vendedor.
            var data = await result
                .Include(x => x.Seller)
                .ThenInclude(s => s.Department)
                .OrderByDescending(x => x.Date) // Ordem antes do agrupamento para garantir o resultado
                .ToListAsync();

            return data.GroupBy(x => x.Seller.Department).ToList();
        }

        public async Task<List<DepartmentSalesViewModel>> FindAggregatedSalesByDepartmentAsync(DateTime? minDate, DateTime? maxDate)
        {
            var query = _context.SalesRecord
                .Include(sr => sr.Seller)
                    .ThenInclude(s => s.Department)
                .AsQueryable();

            // Aplica os filtros de data com UTC
            if (minDate.HasValue)
            {
                var utcMinDate = minDate.Value.ToUniversalTime();
                query = query.Where(sr => sr.Date >= utcMinDate);
            }
            if (maxDate.HasValue)
            {
                var utcMaxDateInclusive = maxDate.Value.AddDays(1).ToUniversalTime();
                query = query.Where(sr => sr.Date < utcMaxDateInclusive);
            }

            // Agrupa por departamento e seleciona apenas o nome e a soma das vendas
            var result = await query
                .GroupBy(sr => sr.Seller.Department) // Agrupa pela entidade Department
                .Select(group => new DepartmentSalesViewModel // Projeta para o ViewModel simples
                {
                    DepartmentName = group.Key.Name,
                    TotalSales = group.Sum(sr => sr.Amount)
                })
                .OrderBy(item => item.DepartmentName) // Ordena por nome do departamento
                .ToListAsync();

            return result;
        }
    }
}