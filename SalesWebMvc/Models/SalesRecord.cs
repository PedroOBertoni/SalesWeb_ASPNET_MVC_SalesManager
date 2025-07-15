using System;
using SalesWebMvc.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesWebMvc.Models
{
    public class SalesRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O Nome do Vendedor é obrigatório")]
        [Display(Name = "Vendedor")]
        public int SellerId { get; set; } // Você pode usar isso se for passar apenas o ID do vendedor

        public Seller Seller { get; set; } // Ou usar esta propriedade se for passar o objeto Seller

        [Required(ErrorMessage = "O Valor da Venda é obrigatório")]
        [Range(1.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que 1")]
        [Display(Name = "Valor")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "A Data da Venda é obrigatória")]
        [DataType(DataType.Date)]
        [Display(Name = "Data da Venda")]
        [CustomValidation(typeof(SalesRecord), nameof(ValidateDate))]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "O Status da Venda é obrigatório")]
        [Display(Name = "Status")]
        public SaleStatus Status { get; set; } 

        // Construtor padrão (sem argumentos) - Boa prática manter se você for usar inicialização de propriedades separada
        public SalesRecord()
        {
        }

        // Construtor que aceita os 4 argumentos que você está passando
        // Ajuste os tipos dos parâmetros para corresponder exatamente ao que você está passando no SeedingService
        public SalesRecord(DateTime date, double amount, SaleStatus status, Seller seller)
        {
            Date = date;
            Amount = amount;
            Status = status;
            Seller = seller;
        }


        public static ValidationResult ValidateDate(DateTime date, ValidationContext context)
        {
            if (date > DateTime.Today) // Usando DateTime.Today para uma validação mais robusta
            {
                return new ValidationResult("A data deve ser igual ou anterior que a data atual.");
            }

            return ValidationResult.Success;
        }
    }
}