using System;

namespace SalesWebMvc.Models.ViewModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public string Message { get; set; }
        public string EntityType { get; set; } // Adicionado para identificar o tipo de entidade no erro

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}