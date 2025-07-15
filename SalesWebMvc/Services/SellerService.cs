using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync(bool includeDepartment = true)
        {
            if (includeDepartment)
            {
                return await _context.Seller
                    .Include(obj => obj.Department)
                    .AsNoTracking()
                    .ToListAsync();
            }
            else
            {
                return await _context.Seller
                    .AsNoTracking()
                    .ToListAsync();
            }
        }

        public async Task InsertAsync(Seller obj)
        {
            // Se você tiver validações ou outras lógicas, elas viriam aqui
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            // Inclui o Department para evitar N+1 selects
            return await _context.Seller
                .Include(obj => obj.Department)
                .FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                var obj = await _context.Seller.FindAsync(id);
                _context.Seller.Remove(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                // Captura a exceção de integridade referencial do banco de dados
                // e lança uma exceção customizada.
                throw new IntegrityException("Não é possível deletar o vendedor porque ele(a) possui vendas vinculadas.");
            }
        }

        public async Task UpdateAsync(Seller obj)
        {
            // Verifica se o objeto existe no banco de dados
            bool hasAny = await _context.Seller.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Id not found");
            }

            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}