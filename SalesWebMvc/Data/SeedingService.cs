using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Models;
using SalesWebMvc.Models.Enums;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace SalesWebMvc.Data
{
    public class SeedingService
    {
        private SalesWebMvcContext _context;

        public SeedingService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (_context.Department.Any()
                || _context.Seller.Any()
                || _context.SalesRecord.Any())
            {
                return; // Aqui nesse if estou verificando se há alguma dessas tabelas no banco, caso sim, ele da o return e já corta o precesso de seed
                // Caso contrário ele segue e faz o processo de seeding
            }

            // Instanciação dos objetos que serão inseridos no banco de dados com o seed
            Department d1 = new Department(1, "Computadores");
            Department d2 = new Department(2, "Eletrônicos");
            Department d3 = new Department(3, "Roupas");
            Department d4 = new Department(4, "Livros");
            Department d5 = new Department(5, "Jogos");
            Department d6 = new Department(6, "Esportes");

            Seller s1 = new Seller(1, "Bruno Oliveira", "bruno@gmail.com", new DateTime(1998, 4, 21, 0, 0, 0, DateTimeKind.Utc), 1000.0, d1);
            Seller s2 = new Seller(2, "Maria Clara", "maria@gmail.com", new DateTime(1979, 12, 31, 0, 0, 0, DateTimeKind.Utc), 3500.0, d2);
            Seller s3 = new Seller(3, "Alexandre Moura", "alexandremoura@gmail.com", new DateTime(1988, 1, 15, 0, 0, 0, DateTimeKind.Utc), 2200.0, d1);
            Seller s4 = new Seller(4, "Marta Soares", "marta@gmail.com", new DateTime(1993, 11, 30, 0, 0, 0, DateTimeKind.Utc), 3000.0, d4);
            Seller s5 = new Seller(5, "Daniel Ribeiro", "daniel@gmail.com", new DateTime(2000, 1, 9, 0, 0, 0, DateTimeKind.Utc), 4000.0, d3);
            Seller s6 = new Seller(6, "Alexandre Silva", "alexandresilva@gmail.com", new DateTime(1997, 3, 4, 0, 0, 0, DateTimeKind.Utc), 3000.0, d2);
            Seller s7 = new Seller(7, "Carlos Souza", "carlos@gmail.com", new DateTime(2000, 7, 30, 0, 0, 0, DateTimeKind.Utc), 5000.0, d6);
            Seller s8 = new Seller(8, "João Matos", "joao@gmail.com", new DateTime(1988, 2, 26, 0, 0, 0, DateTimeKind.Utc), 7200.0, d5);

            SalesRecord r1 = new SalesRecord(new DateTime(2024, 11, 01, 0, 0, 0, DateTimeKind.Utc), 12345.67, SaleStatus.Finalizada, s3);
            SalesRecord r2 = new SalesRecord(new DateTime(2024, 11, 05, 0, 0, 0, DateTimeKind.Utc), 9876.54, SaleStatus.Pendente, s5);
            SalesRecord r3 = new SalesRecord(new DateTime(2024, 11, 09, 0, 0, 0, DateTimeKind.Utc), 5432.10, SaleStatus.Cancelada, s1);
            SalesRecord r4 = new SalesRecord(new DateTime(2024, 11, 13, 0, 0, 0, DateTimeKind.Utc), 18901.23, SaleStatus.Finalizada, s6);
            SalesRecord r5 = new SalesRecord(new DateTime(2024, 11, 17, 0, 0, 0, DateTimeKind.Utc), 3456.78, SaleStatus.Pendente, s2);
            SalesRecord r6 = new SalesRecord(new DateTime(2024, 11, 21, 0, 0, 0, DateTimeKind.Utc), 15678.90, SaleStatus.Cancelada, s4);
            SalesRecord r7 = new SalesRecord(new DateTime(2024, 11, 25, 0, 0, 0, DateTimeKind.Utc), 7890.12, SaleStatus.Finalizada, s3);
            SalesRecord r8 = new SalesRecord(new DateTime(2024, 11, 29, 0, 0, 0, DateTimeKind.Utc), 2345.67, SaleStatus.Pendente, s5);
            SalesRecord r9 = new SalesRecord(new DateTime(2024, 12, 03, 0, 0, 0, DateTimeKind.Utc), 11223.34, SaleStatus.Cancelada, s1);
            SalesRecord r10 = new SalesRecord(new DateTime(2024, 12, 07, 0, 0, 0, DateTimeKind.Utc), 4567.89, SaleStatus.Finalizada, s6);
            SalesRecord r11 = new SalesRecord(new DateTime(2024, 12, 11, 0, 0, 0, DateTimeKind.Utc), 19876.54, SaleStatus.Pendente, s2);
            SalesRecord r12 = new SalesRecord(new DateTime(2024, 12, 15, 0, 0, 0, DateTimeKind.Utc), 6789.01, SaleStatus.Cancelada, s4);
            SalesRecord r13 = new SalesRecord(new DateTime(2024, 12, 19, 0, 0, 0, DateTimeKind.Utc), 14567.89, SaleStatus.Finalizada, s3);
            SalesRecord r14 = new SalesRecord(new DateTime(2024, 12, 23, 0, 0, 0, DateTimeKind.Utc), 8901.23, SaleStatus.Pendente, s5);
            SalesRecord r15 = new SalesRecord(new DateTime(2024, 12, 27, 0, 0, 0, DateTimeKind.Utc), 2000.00, SaleStatus.Cancelada, s1);
            SalesRecord r16 = new SalesRecord(new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Utc), 10112.23, SaleStatus.Finalizada, s6);
            SalesRecord r17 = new SalesRecord(new DateTime(2025, 01, 04, 0, 0, 0, DateTimeKind.Utc), 3344.55, SaleStatus.Pendente, s2);
            SalesRecord r18 = new SalesRecord(new DateTime(2025, 01, 08, 0, 0, 0, DateTimeKind.Utc), 17890.12, SaleStatus.Cancelada, s4);
            SalesRecord r19 = new SalesRecord(new DateTime(2025, 01, 12, 0, 0, 0, DateTimeKind.Utc), 9012.34, SaleStatus.Finalizada, s3);
            SalesRecord r20 = new SalesRecord(new DateTime(2025, 01, 16, 0, 0, 0, DateTimeKind.Utc), 5678.90, SaleStatus.Pendente, s5);
            SalesRecord r21 = new SalesRecord(new DateTime(2025, 01, 20, 0, 0, 0, DateTimeKind.Utc), 13456.78, SaleStatus.Cancelada, s1);
            SalesRecord r22 = new SalesRecord(new DateTime(2025, 01, 24, 0, 0, 0, DateTimeKind.Utc), 2901.23, SaleStatus.Finalizada, s6);
            SalesRecord r23 = new SalesRecord(new DateTime(2025, 01, 28, 0, 0, 0, DateTimeKind.Utc), 16789.01, SaleStatus.Pendente, s2);
            SalesRecord r24 = new SalesRecord(new DateTime(2025, 02, 01, 0, 0, 0, DateTimeKind.Utc), 8901.23, SaleStatus.Cancelada, s4);
            SalesRecord r25 = new SalesRecord(new DateTime(2025, 02, 05, 0, 0, 0, DateTimeKind.Utc), 4567.89, SaleStatus.Finalizada, s3);
            SalesRecord r26 = new SalesRecord(new DateTime(2025, 02, 09, 0, 0, 0, DateTimeKind.Utc), 19012.34, SaleStatus.Pendente, s5);
            SalesRecord r27 = new SalesRecord(new DateTime(2025, 02, 13, 0, 0, 0, DateTimeKind.Utc), 5678.90, SaleStatus.Cancelada, s1);
            SalesRecord r28 = new SalesRecord(new DateTime(2025, 02, 17, 0, 0, 0, DateTimeKind.Utc), 12345.67, SaleStatus.Finalizada, s6);
            SalesRecord r29 = new SalesRecord(new DateTime(2025, 02, 21, 0, 0, 0, DateTimeKind.Utc), 7890.12, SaleStatus.Pendente, s2);
            SalesRecord r30 = new SalesRecord(new DateTime(2025, 02, 25, 0, 0, 0, DateTimeKind.Utc), 20000.00, SaleStatus.Cancelada, s4);


            // Adicionando os objetos no banco de dados
            _context.Department.AddRange(d1, d2, d3, d4, d5, d6);

            _context.Seller.AddRange(s1, s2, s3, s4, s5, s6, s7, s8);

            _context.SalesRecord.AddRange(
                r1, r2, r3, r4, r5, r6, r7, r8, r9, r10,
                r11, r12, r13, r14, r15, r16, r17, r18, r19, r20,
                r21, r22, r23, r24, r25, r26, r27, r28, r29, r30
            );


            // Salvando alterações realizadas no banco de dados
            _context.SaveChanges();
        }
    }
}