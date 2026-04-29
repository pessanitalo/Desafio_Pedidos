using System.ComponentModel.DataAnnotations;

namespace DesafioPedido.Application.DTOs
{
    public class ClienteDTO
    {
        public int ClienteId { get; set; }
        [Required(ErrorMessage = "Nome é obrigatório.")]
        [MinLength(3, ErrorMessage = "Nome deve ter no mínimo 3 caracteres.")]
        [MaxLength(150, ErrorMessage = "Nome deve ter no máximo 150 caracteres.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        [MaxLength(255, ErrorMessage = "Email deve ter no máximo 255 caracteres.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Telefone é obrigatório.")]
        [RegularExpression(@"^\(\d{2}\) \d{2} \d{5}-\d{4}$", ErrorMessage = "Formato inválido. Use (55) 11 99999-9999")]
        public string Telefone { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
