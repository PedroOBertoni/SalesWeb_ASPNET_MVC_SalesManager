using System.Collections.Generic;

namespace SalesWebMvc.Models.ViewModels
{
    public class SellerFormViewModel // Classe qu epossui um objeto Seller e a lista com todos os departamentos, para que seja possível implementar
                                     // A chave estrangeira DepartamentId no Seller
    {
        public Seller Seller { get; set; }
        public ICollection<Department> Departments { get; set; }
    }
}
