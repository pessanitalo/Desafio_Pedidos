using System.ComponentModel.DataAnnotations;

namespace DesafioPedido.Application.DTOs
{
    public class ProdutoDto
    {
        public int ProdutoId { get; set; }


        [Required(ErrorMessage = "Nome é obrigatório.")]
        [MinLength(3, ErrorMessage = "Nome deve ter no mínimo 3 caracteres.")]
        [MaxLength(150, ErrorMessage = "Nome deve ter no máximo 150 caracteres.")]
        public string Nome { get; set; }



        [Required(ErrorMessage = "Descrição é obrigatório.")]
        [MinLength(3, ErrorMessage = "Descrição deve ter no mínimo 3 caracteres.")]
        [MaxLength(150, ErrorMessage = "Descrição deve ter no máximo 150 caracteres.")]
        public string Descricao { get; set; }


        [Required(ErrorMessage = "Preço é obrigatório.")]
        [Range(0.01, 999999.99, ErrorMessage = "Preço deve ser entre R$ 0,01 e R$ 999.999,99.")]
        public decimal Preco { get; set; }



        [Required(ErrorMessage = "Quantidade é obrigatória.")]
        [Range(1, 9999, ErrorMessage = "Quantidade deve ser entre 1 e 9999.")]
        public int QuantidadeEstoque { get; set; }
    }
}
