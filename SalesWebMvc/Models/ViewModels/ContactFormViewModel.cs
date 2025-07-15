using System.ComponentModel.DataAnnotations;

namespace SalesWeb.ViewModels
{
    public class ContactFormViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Por favor, insira um endereço de e-mail válido.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Por favor, insira um número de telefone válido.")]
        public string Phone { get; set; } // Opcional, pode ser nulo

        [Required(ErrorMessage = "A mensagem é obrigatória.")]
        [StringLength(1000, ErrorMessage = "A mensagem não pode exceder 1000 caracteres.")]
        public string Message { get; set; }
    }
}